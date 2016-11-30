using Aura.Framework.Settings.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aura.Framework.Settings.Models.Formatting
{
    public class DataDefaultFormatter : DataFormatter
    {
        private ParseConfiguration _configuration;

        #region Initialization

        public DataDefaultFormatter() : this(new ParseConfiguration())
        {
        }

        public DataDefaultFormatter(ParseConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");
            this.Configuration = configuration;
        }

        #endregion Initialization

        public virtual string IniDataToString(Data iniData)
        {
            var sb = new StringBuilder();

            if (Configuration.AllowKeysWithoutSection)
            {
                // Write global key/value data
                WriteKeyValueData(iniData.Global, sb);
            }

            //Write sections
            foreach (SectionData section in iniData.Sections)
            {
                //Write current section
                WriteSection(section, sb);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Configuration used to write a config file with the proper
        /// delimiter characters and data.
        /// </summary>
        /// <remarks>
        /// If the <see cref="Data"/> instance was created by a parser,
        /// this instance is a copy of the <see cref="ParseConfiguration"/> used
        /// by the parser (i.e. different objects instances)
        /// If this instance is created programatically without using a parser, this
        /// property returns an instance of <see cref=" ParseConfiguration"/>
        /// </remarks>
        public ParseConfiguration Configuration
        {
            get { return _configuration; }
            set { _configuration = value.Clone(); }
        }

        #region Helpers

        private void WriteSection(SectionData section, StringBuilder sb)
        {
            // Write blank line before section, but not if it is the first line
            if (sb.Length > 0) sb.Append(Configuration.NewLineString);

            // Leading comments
            WriteComments(section.LeadingComments, sb);

            //Write section name
            sb.Append(string.Format("{0}{1}{2}{3}",
                Configuration.SectionStartChar,
                section.SectionName,
                Configuration.SectionEndChar,
                Configuration.NewLineString));

            WriteKeyValueData(section.Keys, sb);

            // Trailing comments
            WriteComments(section.TrailingComments, sb);
        }

        private void WriteKeyValueData(KeyDataCollection keyDataCollection, StringBuilder sb)
        {
            foreach (KeyData keyData in keyDataCollection)
            {
                // Add a blank line if the key value pair has comments
                if (keyData.Comments.Count > 0) sb.Append(Configuration.NewLineString);

                // Write key comments
                WriteComments(keyData.Comments, sb);

                //Write key and value
                sb.Append(string.Format("{0}{3}{1}{3}{2}{4}",
                    keyData.KeyName,
                    Configuration.KeyValueAssigmentChar,
                    keyData.Value,
                    Configuration.AssigmentSpacer,
                    Configuration.NewLineString));
            }
        }

        private void WriteComments(List<string> comments, StringBuilder sb)
        {
            foreach (string comment in comments)
                sb.Append(string.Format("{0}{1}{2}", Configuration.CommentString, comment, Configuration.NewLineString));
        }

        #endregion Helpers
    }
}