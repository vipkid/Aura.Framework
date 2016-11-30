namespace Aura.Framework.Settings.Helpers
{
    /// <summary>
    /// A class that contains functionality that asserts.
    /// </summary>
    internal static class Assert
    {
        #region Methods

        /// <summary>
        /// Asserts that a strings has no blank spaces.
        /// </summary>
        internal static bool CheckString(string s)
        {
            return !s.Contains(" ");
        }

        #endregion Methods
    }
}