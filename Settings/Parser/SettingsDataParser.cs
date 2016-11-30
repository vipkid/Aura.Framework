using Aura.Framework.Settings.Exceptions;
using Aura.Framework.Settings.Models;
using Aura.Framework.Settings.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Aura.Framework.Settings.Parser
{
    /// <summary>
    /// Responsible for parsing an string from a config file, and creating an <see cref="Data"/> structure.
    /// </summary>
    public class SettingsDataParser : IDisposable
    {
        #region Privates

        /// <summary>
        /// Holds a list of the exceptions catched while parsing
        /// </summary>
        private List<Exception> _ErrorExceptions;

        #endregion Privates

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseConfiguration"/> class.
        /// </summary>
        public SettingsDataParser() : this(new ParseConfiguration())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseConfiguration"/> class.
        /// </summary>
        public SettingsDataParser(ParseConfiguration parserConfiguration)
        {
            if (parserConfiguration == null)
                throw new ArgumentNullException("parserConfiguration");

            Configuration = parserConfiguration;

            _ErrorExceptions = new List<Exception>();
        }

        #endregion Initialization

        #region State

        /// <summary>
        /// Configuration that defines the behaviour and constraints that the parser must follow.
        /// </summary>
        public virtual ParseConfiguration Configuration { get; protected set; }

        /// <summary>
        /// True is the parsing operation encounter any problem.
        /// </summary>
        public bool HasError { get { return _ErrorExceptions.Count > 0; } }

        /// <summary>
        /// Returns the list of errors found while parsing the ini file.
        /// </summary>
        public ReadOnlyCollection<Exception> Errors { get { return _ErrorExceptions.AsReadOnly(); } }

        #endregion State

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        #endregion Methods

        #region Operations

        /// <summary>
        /// Parses a string containing valid config data.
        /// </summary>
        public Data Parse(string iniDataString)
        {
            Data iniData = Configuration.CaseInsensitive ? new DataCaseInsensitive() : new Data();
            iniData.Configuration = this.Configuration.Clone();

            if (string.IsNullOrEmpty(iniDataString))
            {
                return iniData;
            }

            _ErrorExceptions.Clear();
            _CurrentCommentListTemp.Clear();
            _CurrentSectionNameTemp = null;

            try
            {
                var lines = iniDataString.Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);
                for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
                {
                    var line = lines[lineNumber];

                    if (line.Trim() == String.Empty) continue;

                    try
                    {
                        ProcessLine(line, iniData);
                    }
                    catch (Exception ex)
                    {
                        var errorEx = new ParsingException(ex.Message, lineNumber + 1, line, ex);
                        if (Configuration.ThrowExceptionsOnError)
                        {
                            throw errorEx;
                        }
                        else
                        {
                            _ErrorExceptions.Add(errorEx);
                        }
                    }
                }

                // Orphan comments, assing to last section/key value
                if (_CurrentCommentListTemp.Count > 0)
                {
                    // Check if there are actually sections in the file
                    if (iniData.Sections.Count > 0)
                    {
                        iniData.Sections.GetSectionData(_CurrentSectionNameTemp).TrailingComments
                            .AddRange(_CurrentCommentListTemp);
                    }
                    // No sections, put the comment in the last key value pair
                    // but only if the ini file contains at least one key-value pair
                    else if (iniData.Global.Count > 0)
                    {
                        iniData.Global.GetLast().Comments
                            .AddRange(_CurrentCommentListTemp);
                    }

                    _CurrentCommentListTemp.Clear();
                }
            }
            catch (Exception ex)
            {
                _ErrorExceptions.Add(ex);
                if (Configuration.ThrowExceptionsOnError)
                {
                    throw;
                }
            }

            if (HasError) return null;
            return (Data)iniData.Clone();
        }

        #endregion Operations

        #region Template Method Design Pattern

        /// <summary>
        /// Checks if a given string contains a comment.
        /// </summary>
        protected virtual bool LineContainsAComment(string line)
        {
            return !string.IsNullOrEmpty(line)
                && Configuration.CommentRegex.Match(line).Success;
        }

        /// <summary>
        /// Checks if a given string represents a section delimiter.
        /// </summary>
        protected virtual bool LineMatchesASection(string line)
        {
            return !string.IsNullOrEmpty(line)
                && Configuration.SectionRegex.Match(line).Success;
        }

        /// <summary>
        /// Checks if a given string represents a key / value pair.
        /// </summary>
        protected virtual bool LineMatchesAKeyValuePair(string line)
        {
            return !string.IsNullOrEmpty(line) && line.Contains(Configuration.KeyValueAssigmentChar.ToString());
        }

        /// <summary>
        /// Removes a comment from a string if exist, and returns the string without the comment substring.
        /// </summary>
        protected virtual string ExtractComment(string line)
        {
            string comment = Configuration.CommentRegex.Match(line).Value.Trim();

            _CurrentCommentListTemp.Add(comment.Substring(1, comment.Length - 1));

            return line.Replace(comment, "").Trim();
        }

        /// <summary>
        /// Processes one line and parses the data found in that line (section or key/value pair who may or may not have comments)
        /// </summary>
        protected virtual void ProcessLine(string currentLine, Data currentIniData)
        {
            currentLine = currentLine.Trim();

            // Extract comments from current line and store them in a tmp field
            if (LineContainsAComment(currentLine))
            {
                currentLine = ExtractComment(currentLine);
            }

            // By default comments must span a complete line (i.e. the comment character
            // must be located at the beginning of a line, so it seems that the following
            // check is not needed.
            // But, if the comment parsing behaviour is changed in a derived class e.g. to
            // to allow parsing comment characters in the middle of a line, the implementor
            // will be forced to rewrite this complete method.
            // That was actually the behaviour for parsing comments
            // in earlier versions of the library, so checking if the current line is empty
            // (meaning the complete line was a comment) is future-proof.

            // If the entire line was a comment now should be empty,
            // so no further processing is needed.
            if (currentLine == String.Empty)
                return;

            //Process sections
            if (LineMatchesASection(currentLine))
            {
                ProcessSection(currentLine, currentIniData);
                return;
            }

            //Process keys
            if (LineMatchesAKeyValuePair(currentLine))
            {
                ProcessKeyValuePair(currentLine, currentIniData);
                return;
            }

            if (Configuration.SkipInvalidLines)
                return;

            throw new ParsingException(
                "Unknown file format. Couldn't parse the line: '" + currentLine + "'.");
        }

        /// <summary>
        /// Proccess a string which contains a config section.
        /// </summary>
        protected virtual void ProcessSection(string line, Data currentIniData)
        {
            // Get section name with delimiters from line...
            string sectionName = Configuration.SectionRegex.Match(line).Value.Trim();

            // ... and remove section's delimiters to get just the name
            sectionName = sectionName.Substring(1, sectionName.Length - 2).Trim();

            // Check that the section's name is not empty
            if (sectionName == string.Empty)
            {
                throw new ParsingException("Section name is empty");
            }

            // Temporally save section name.
            _CurrentSectionNameTemp = sectionName;

            //Checks if the section already exists
            if (currentIniData.Sections.ContainsSection(sectionName))
            {
                if (Configuration.AllowDuplicateSections)
                {
                    return;
                }

                throw new ParsingException(string.Format("Duplicate section with name '{0}' on line '{1}'", sectionName, line));
            }

            // If the section does not exists, add it to the ini data
            currentIniData.Sections.AddSection(sectionName);

            // Save comments read until now and assign them to this section
            currentIniData.Sections.GetSectionData(sectionName).LeadingComments = _CurrentCommentListTemp;
            _CurrentCommentListTemp.Clear();
        }

        /// <summary>
        /// Processes a string containing a config key/value pair.
        /// </summary>
        protected virtual void ProcessKeyValuePair(string line, Data currentIniData)
        {
            // Get key and value data
            string key = ExtractKey(line);
            string value = ExtractValue(line);

            // Check if we haven't read any section yet
            if (string.IsNullOrEmpty(_CurrentSectionNameTemp))
            {
                if (!Configuration.AllowKeysWithoutSection)
                {
                    throw new ParsingException("key value pairs must be enclosed in a section");
                }

                AddKeyToKeyValueCollection(key, value, currentIniData.Global, "global");
            }
            else
            {
                var currentSection = currentIniData.Sections.GetSectionData(_CurrentSectionNameTemp);

                AddKeyToKeyValueCollection(key, value, currentSection.Keys, _CurrentSectionNameTemp);
            }
        }

        /// <summary>
        /// Extracts the key portion of a string containing a key/value pair.
        /// </summary>
        protected virtual string ExtractKey(string s)
        {
            int index = s.IndexOf(Configuration.KeyValueAssigmentChar, 0);

            return s.Substring(0, index).Trim();
        }

        /// <summary>
        /// Extracts the value portion of a string containing a key/value pair.
        /// </summary>
        protected virtual string ExtractValue(string s)
        {
            int index = s.IndexOf(Configuration.KeyValueAssigmentChar, 0);

            return s.Substring(index + 1, s.Length - index - 1).Trim();
        }

        /// <summary>
        /// Abstract Method that decides what to do in case we are trying to add a duplicated key to a section.
        /// </summary>
        protected virtual void HandleDuplicatedKeyInCollection(string key, string value, KeyDataCollection keyDataCollection, string sectionName)
        {
            if (!Configuration.AllowDuplicateKeys)
            {
                throw new ParsingException(string.Format("Duplicated key '{0}' found in section '{1}", key, sectionName));
            }
            else if (Configuration.OverrideDuplicateKeys)
            {
                keyDataCollection[key] = value;
            }
        }

        #endregion Template Method Design Pattern

        #region Helpers

        /// <summary>
        /// Adds a key to a concrete <see cref="KeyDataCollection"/> instance, checking if duplicate keys are allowed in the configuration.
        /// </summary>
        private void AddKeyToKeyValueCollection(string key, string value, KeyDataCollection keyDataCollection, string sectionName)
        {
            // Check for duplicated keys
            if (keyDataCollection.ContainsKey(key))
            {
                // We already have a key with the same name defined in the current section
                HandleDuplicatedKeyInCollection(key, value, keyDataCollection, sectionName);
            }
            else
            {
                // Save the keys
                keyDataCollection.AddKey(key, value);
            }

            keyDataCollection.GetKeyData(key).Comments = _CurrentCommentListTemp;
            _CurrentCommentListTemp.Clear();
        }

        #endregion Helpers

        #region Fields

        /// <summary>
        /// Temporary list of comments.
        /// </summary>
        private readonly List<string> _CurrentCommentListTemp = new List<string>();

        /// <summary>
        /// Temporary var with the name of the seccion which is being process.
        /// </summary>
        private string _CurrentSectionNameTemp;

        #endregion Fields
    }
}