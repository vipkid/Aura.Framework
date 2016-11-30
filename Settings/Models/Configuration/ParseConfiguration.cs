using System;
using System.Text.RegularExpressions;

namespace Aura.Framework.Settings.Models.Configuration
{
    /// <summary>
    /// Defines data for a Parser configuration object.
    /// </summary>
    public class ParseConfiguration : ICloneable
    {
        #region Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseConfiguration"/> class.
        /// </summary>
        public ParseConfiguration()
        {
            CommentString = ";";
            SectionStartChar = '[';
            SectionEndChar = ']';
            KeyValueAssigmentChar = '=';
            AssigmentSpacer = " ";
            NewLineString = Environment.NewLine;
            ConcatenateDuplicateKeys = false;
            AllowKeysWithoutSection = true;
            AllowDuplicateKeys = false;
            AllowDuplicateSections = false;
            ThrowExceptionsOnError = true;
            SkipInvalidLines = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseConfiguration"/> class.
        /// </summary>
        public ParseConfiguration(ParseConfiguration ori)
        {
            AllowDuplicateKeys = ori.AllowDuplicateKeys;
            OverrideDuplicateKeys = ori.OverrideDuplicateKeys;
            AllowDuplicateSections = ori.AllowDuplicateSections;
            AllowKeysWithoutSection = ori.AllowKeysWithoutSection;

            SectionStartChar = ori.SectionStartChar;
            SectionEndChar = ori.SectionEndChar;
            CommentString = ori.CommentString;
            ThrowExceptionsOnError = ori.ThrowExceptionsOnError;

            // Regex values should recreate themselves.
        }

        #endregion Initialization

        #region ConfigParserConfiguration

        public Regex CommentRegex { get; set; }

        public Regex SectionRegex { get; set; }

        /// <summary>
        /// Sets the char that defines the start of a section name.
        /// </summary>
        public char SectionStartChar
        {
            get { return _SectionStartChar; }
            set
            {
                _SectionStartChar = value;
                RecreateSectionRegex(_SectionStartChar);
            }
        }

        /// <summary>
        /// Sets the char that defines the end of a section name.
        /// </summary>
        public char SectionEndChar
        {
            get { return _SectionEndChar; }
            set
            {
                _SectionEndChar = value;
                RecreateSectionRegex(_SectionEndChar);
            }
        }

        /// <summary>
        /// Retrieving section / keys by name is done with a case-insensitive search.
        /// </summary>
        public bool CaseInsensitive { get; set; }

        /// <summary>
        /// Sets the char that defines the start of a comment. A comment spans from the comment character to the end of the line.
        /// </summary>
        [Obsolete("Please use the CommentString property")]
        public char CommentChar
        {
            get { return CommentString[0]; }
            set { CommentString = value.ToString(); }
        }

        /// <summary>
        /// Sets the string that defines the start of a comment. A comment spans from the mirst matching comment string to the end of the line.
        /// </summary>
        public string CommentString
        {
            get { return _CommentString ?? string.Empty; }
            set
            {
                // Sanitarize special characters for a regex
                foreach (var specialChar in _StringSpecialRegexChars)
                {
                    value = value.Replace(new String(specialChar, 1), @"\" + specialChar);
                }

                CommentRegex = new Regex(string.Format(_StringCommentRegex, value));
                _CommentString = value;
            }
        }

        /// <summary>
        /// Gets or sets the string to use as new line string when formating a configData structure using a IConfigDataFormatter. Parsing an config-file accepts any new line character (Unix/windows).
        /// </summary>
        public string NewLineString
        {
            get; set;
        }

        /// <summary>
        /// Sets the char that defines a value assigned to a key.
        /// </summary>
        public char KeyValueAssigmentChar { get; set; }

        /// <summary>
        /// Sets the string around the <see cref="KeyValueAssigmentChar"/> property.
        /// </summary>
        public string AssigmentSpacer { get; set; }

        /// <summary>
        /// Allows having keys in the file that don't belong to any section, i.e. allows defining keys before defining a section. If set to <c>false</c> and keys without a section are defined, the <see cref="ConfigDataParser"/> will stop with an error.
        /// </summary>
        public bool AllowKeysWithoutSection { get; set; }

        /// <summary>
        /// If set to <c>false</c> and the <see cref="ConfigDataParser"/> finds duplicate keys in a section the parser will stop with an error. If set to <c>true</c>, duplicated keys are allowed in the file. The value of the duplicate key will be the last value asigned to the key in the file.
        /// </summary>
        public bool AllowDuplicateKeys { get; set; }

        /// <summary>
        /// Only used if <see cref="AllowDuplicateKeys"/> is also <c>true</c> If set to <c>true</c> when the parser finds a duplicate key, it overrites the previous value, so the key will always contain the value of the last key readed in the file. If set to <c>false</c> the first readed value is preserved, so the key will always contain the value of the first key readed in the file.
        /// </summary>
        public bool OverrideDuplicateKeys { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether duplicate keys are concatenate together by <see cref="ConcatenateSeparator"/>.
        /// </summary>
        public bool ConcatenateDuplicateKeys { get; set; }

        /// <summary>
        /// If <c>true</c> the <see cref="ConfigDataParser"/> instance will thrown an exception if an error is found. If <c>false</c> the parser will just stop execution and return a null value.
        /// </summary>
        public bool ThrowExceptionsOnError { get; set; }

        /// <summary>
        /// If set to <c>false</c> and the <see cref="ConfigDataParser"/> finds a duplicate section the parser will stop with an error. If set to <c>true</c>, duplicated sections are allowed in the file, but only a <see cref="SectionData"/> element will be created in the <see cref="Data.Sections"/>  collection.
        /// </summary>
        public bool AllowDuplicateSections { get; set; }

        /// <summary>
        /// Represents a <see cref="bool"/> value whether the configuration is set to skip invalid lines in the config file that has been parsed.
        /// </summary>
        public bool SkipInvalidLines { get; set; }

        #endregion ConfigParserConfiguration

        #region Fields

        private char _SectionStartChar;
        private char _SectionEndChar;
        private string _CommentString;

        #endregion Fields

        #region Constants

        protected const string _StringCommentRegex = @"^{0}(.*)";
        protected const string _StringSectionRegexStart = @"^(\s*?)";
        protected const string _StringSectionRegexMiddle = @"{1}\s*[\p{L}\p{P}\p{M}_\""\'\{\}\#\+\;\*\%\(\)\=\?\&\$\,\:\/\.\-\w\d\s\\\~]+\s*";
        protected const string _StringSectionRegexEnd = @"(\s*?)$";
        protected const string _StringKeyRegex = @"^(\s*[_\.\d\w]*\s*)";
        protected const string _StringValueRegex = @"([\s\d\w\W\.]*)$";
        protected const string _StringSpecialRegexChars = @"[]\^$.|?*+()";

        #endregion Constants

        #region Helpers

        private void RecreateSectionRegex(char value)
        {
            if (char.IsControl(value)
                || char.IsWhiteSpace(value)
                || CommentString.Contains(new string(new[] { value }))
                || value == KeyValueAssigmentChar)
                throw new Exception(string.Format("Invalid character for section delimiter: '{0}", value));

            string builtRegexString = _StringSectionRegexStart;

            if (_StringSpecialRegexChars.Contains(new string(_SectionStartChar, 1)))
                builtRegexString += "\\" + _SectionStartChar;
            else builtRegexString += _SectionStartChar;

            builtRegexString += _StringSectionRegexMiddle;

            if (_StringSpecialRegexChars.Contains(new string(_SectionEndChar, 1)))
                builtRegexString += "\\" + _SectionEndChar;
            else
                builtRegexString += _SectionEndChar;

            builtRegexString += _StringSectionRegexEnd;

            SectionRegex = new Regex(builtRegexString);
        }

        #endregion Helpers

        #region Overrides

        /// <summary>
        /// Returns the has-code.
        /// </summary>
        public override int GetHashCode()
        {
            var hash = 27;
            foreach (var property in GetType().GetProperties())
            {
                hash = (hash * 7) + property.GetValue(this, null).GetHashCode();
            }

            return hash;
        }

        /// <summary>
        /// Returns if the configuration is the same as the given parameter.
        /// </summary>
        public override bool Equals(object obj)
        {
            var copyObj = obj as ParseConfiguration;
            if (copyObj == null) return false;

            var oriType = this.GetType();
            try
            {
                foreach (var property in oriType.GetProperties())
                {
                    if (property.GetValue(copyObj, null).Equals(property.GetValue(this, null)))
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion Overrides

        #region ICloneable Members

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        public ParseConfiguration Clone()
        {
            return this.MemberwiseClone() as ParseConfiguration;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion ICloneable Members
    }
}