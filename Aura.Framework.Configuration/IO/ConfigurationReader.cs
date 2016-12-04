using Aura.Framework.Configurations.Exceptions;
using Aura.Framework.Configurations.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Aura.Framework.Configurations.IO
{
    /// <summary>
    /// Represents a class that can read configuration files.
    /// </summary>
    internal static class ConfigurationReader
    {
        #region Static Methods

        /// <summary>
        /// Reads a file and returns a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public static Configuration ReadFromString(string source)
        {
            int lineNumber = 0;

            var config = new Configuration();
            Section currentSection = null;
            var preComments = new List<Comment>();

            using (var reader = new StringReader(source))
            {
                string line = null;

                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;

                    line = line.Trim();

                    if (string.IsNullOrEmpty(line))
                        continue;

                    int commentIndex = 0;
                    var comment = ParseComment(line, out commentIndex);

                    if (!Configuration.IgnorePreComments && commentIndex == 0)
                    {
                        preComments.Add(comment.Value);
                        continue;
                    }
                    else if (!Configuration.IgnoreInlineComments && commentIndex > 0)
                    {
                        line = line.Remove(commentIndex).Trim();
                    }

                    if (line.StartsWith("["))
                    {
                        currentSection = ParseSection(line, lineNumber);

                        if (!Configuration.IgnoreInlineComments)
                            currentSection.Comment = comment;

                        if (!Configuration.IgnorePreComments && preComments.Count > 0)
                        {
                            currentSection._Comments = new List<Comment>(preComments);
                            preComments.Clear();
                        }

                        config._Sections.Add(currentSection);
                    }
                    else
                    {
                        var setting = ParseSetting(line, lineNumber);

                        if (!Configuration.IgnoreInlineComments)
                            setting.Comment = comment;

                        if (currentSection == null)
                        {
                            throw new ParseException(string.Format(
                                "The setting '{0}' has to be in a section.",
                                setting.Name), lineNumber);
                        }

                        if (!Configuration.IgnorePreComments && preComments.Count > 0)
                        {
                            setting._Comments = new List<Comment>(preComments);
                            preComments.Clear();
                        }

                        currentSection.Add(setting);
                    }
                }
            }

            return config;
        }

        /// <summary>
        /// Returns a <see cref="bool"/> value whether the current line from a specific start index is between quotes.
        /// </summary>
        private static bool HasQuoteMarks(string line, int startIndex)
        {
            int i = startIndex;
            bool left = false;

            while (--i >= 0)
            {
                if (line[i] == '\"')
                {
                    left = true;
                    break;
                }
            }

            bool right = (line.IndexOf('\"', startIndex) > 0);

            return (left && right);
        }

        /// <summary>
        /// Reads a comment and returns a new instance of the <see cref="Comment"/> class. Note that this can be <see cref="Nullable"/>.
        /// </summary>
        private static Comment? ParseComment(string line, out int commentIndex)
        {
            Comment? comment = null;
            commentIndex = -1;

            do
            {
                commentIndex = line.IndexOfAny(Configuration.ValidCommentChars, commentIndex + 1);

                if (commentIndex < 0)
                    break;
                if (commentIndex >= 1 && line[commentIndex - 1] == '\\')
                    return null;
                if (HasQuoteMarks(line, commentIndex))
                    continue;

                comment = new Comment(
                    value: line.Substring(commentIndex + 1).Trim(),
                    symbol: line[commentIndex]);

                break;
            }
            while (commentIndex >= 0);

            return comment;
        }

        /// <summary>
        /// Returns a new instance of the <see cref="Section"/> class.
        /// </summary>
        private static Section ParseSection(string line, int lineNumber)
        {
            line = line.Trim();

            int closingBracketIndex = line.IndexOf(']');

            if (closingBracketIndex < 0)
                throw new ParseException("closing bracket missing.", lineNumber);

            if ((line.Length - 1) > closingBracketIndex)
            {
                string unwantedToken = line.Substring(closingBracketIndex + 1);

                throw new ParseException(string.Format(
                    "unexpected token '{0}'", unwantedToken),
                    lineNumber);
            }

            string sectionName = line.Substring(1, line.Length - 2).Trim();

            return new Section(sectionName);
        }

        /// <summary>
        /// Returns a new instance of the <see cref="Setting"/> class.
        /// </summary>
        private static Setting ParseSetting(string line, int lineNumber)
        {
            int indexOfAssignOp = line.IndexOf('=');

            if (indexOfAssignOp < 0)
                throw new ParseException("setting assignment expected.", lineNumber);

            string settingName = line.Substring(0, indexOfAssignOp).Trim();
            string settingValue = line.Substring(indexOfAssignOp + 1);
            settingValue = settingValue.Trim();

            if (string.IsNullOrEmpty(settingName))
                throw new ParseException("setting name expected.", lineNumber);

            if (settingValue == null)
                settingValue = string.Empty;

            return new Setting(settingName, settingValue);
        }

        /// <summary>
        /// Reads a file with a <see cref="BinaryReader"/> and returns a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public static Configuration ReadFromBinaryStream(Stream stream, BinaryReader reader)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (reader == null)
                reader = new BinaryReader(stream);

            var config = new Configuration();

            int sectionCount = reader.ReadInt32();

            for (int i = 0; i < sectionCount; ++i)
            {
                string sectionName = reader.ReadString();
                int settingCount = reader.ReadInt32();

                var section = new Section(sectionName);

                ReadCommentsBinary(reader, section);

                for (int j = 0; j < settingCount; j++)
                {
                    var setting = new Setting(
                        reader.ReadString(),
                        reader.ReadString());

                    ReadCommentsBinary(reader, setting);

                    section.Add(setting);
                }

                config.Add(section);
            }

            return config;
        }

        /// <summary>
        /// Reads comments from the <see cref="BinaryReader"/>.
        /// </summary>
        private static void ReadCommentsBinary(BinaryReader reader, ConfigurationElement element)
        {
            bool hasComment = reader.ReadBoolean();
            if (hasComment)
            {
                char symbol = reader.ReadChar();
                string commentValue = reader.ReadString();
                element.Comment = new Comment(commentValue, symbol);
            }

            int preCommentCount = reader.ReadInt32();

            if (preCommentCount > 0)
            {
                element._Comments = new List<Comment>(preCommentCount);

                for (int i = 0; i < preCommentCount; ++i)
                {
                    char symbol = reader.ReadChar();
                    string commentValue = reader.ReadString();
                    element._Comments.Add(new Comment(commentValue, symbol));
                }
            }
        }

        #endregion Static Methods
    }
}