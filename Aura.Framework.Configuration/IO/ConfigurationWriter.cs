using Aura.Framework.Configurations.Overrides;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Aura.Framework.Configurations.IO
{
    /// <summary>
    /// Provides functionality for writing a <see cref="Configuration"/> instance.
    /// </summary>
    internal static class ConfigurationWriter
    {
        #region Methods

        /// <summary>
        /// Writes a <see cref="Configuration"/> with an <see cref="Encoding"/>.
        /// </summary>
        public static void WriteToStreamTextual(Configuration cfg, Stream stream, Encoding encoding)
        {
            Debug.Assert(cfg != null);

            if (stream == null)
                throw new ArgumentNullException("stream");

            if (encoding == null)
                encoding = new UTF8Encoding();

            var sb = new StringBuilder();

            bool isFirstSection = true;

            foreach (var section in cfg)
            {
                if (!isFirstSection)
                    sb.AppendLine();

                if (!isFirstSection && section._Comments != null && section._Comments.Count > 0)
                    sb.AppendLine();

                sb.AppendLine(section.ToString(true));

                foreach (var setting in section)
                {
                    if (setting._Comments != null && setting._Comments.Count > 0)
                        sb.AppendLine();

                    sb.AppendLine(setting.ToString(true));
                }

                isFirstSection = false;
            }

            string str = sb.ToString();

            var byteBuffer = new byte[encoding.GetByteCount(str)];
            int byteCount = encoding.GetBytes(str, 0, str.Length, byteBuffer, 0);

            stream.Write(byteBuffer, 0, byteCount);
            stream.Flush();
        }

        /// <summary>
        /// Writes a <see cref="Configuration"/> with a <see cref="BinaryWriter"/>.
        /// </summary>
        public static void WriteToStreamBinary(Configuration cfg, Stream stream, BinaryWriter writer)
        {
            Debug.Assert(cfg != null);

            if (stream == null)
                throw new ArgumentNullException("stream");

            if (writer == null)
                writer = new NonClosingBinaryWriter(stream);

            writer.Write(cfg.SectionCount);

            foreach (var section in cfg)
            {
                writer.Write(section.Name);
                writer.Write(section.SettingCount);

                WriteCommentsBinary(writer, section);

                foreach (var setting in section)
                {
                    writer.Write(setting.Name);
                    writer.Write(setting.StringValue);

                    WriteCommentsBinary(writer, setting);
                }
            }

            writer.Close();
        }

        /// <summary>
        /// Writes comments for a binary stream.
        /// </summary>
        private static void WriteCommentsBinary(BinaryWriter writer, ConfigurationElement element)
        {
            var commentNullable = element.Comment;

            writer.Write(commentNullable.HasValue);
            if (commentNullable.HasValue)
            {
                var comment = commentNullable.Value;
                writer.Write(comment.Symbol);
                writer.Write(comment.Value);
            }

            var preComments = element._Comments;
            bool hasPreComments = (preComments != null && preComments.Count > 0);

            writer.Write(hasPreComments ? preComments.Count : 0);

            if (hasPreComments)
            {
                foreach (var preComment in preComments)
                {
                    writer.Write(preComment.Symbol);
                    writer.Write(preComment.Value);
                }
            }
        }

        #endregion Methods
    }
}