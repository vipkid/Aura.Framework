namespace Aura.Framework.Configurations.Models
{
    /// <summary>
    /// Represents a comment in a configuration.
    /// </summary>
    public struct Comment
    {
        #region Fields

        /// <summary>
        /// The string value of the comment.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The delimiting symbol of the comment.
        /// </summary>
        public char Symbol { get; set; }

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> structure, using the first element in <see cref="Configuration.ValidCommentChars"/> as the comment symbol.
        /// </summary>
        public Comment(string value)
        {
            Value = value;
            Symbol = Configuration.ValidCommentChars[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> structure.
        /// </summary>
        public Comment(string value, char symbol)
        {
            Value = value;
            Symbol = symbol;
        }

        #endregion Constructors

        #region Overrides

        /// <summary>
        /// Gets the string representation of the comment.
        /// </summary>
        public override string ToString()
        {
            char symbol = Symbol;
            return string.Join(
                System.Environment.NewLine,
                System.Array.ConvertAll(
                    (Value ?? string.Empty).Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None),
                    s => string.Format("{0} {1}", symbol, s)
                )
            );
        }

        #endregion Overrides
    }
}