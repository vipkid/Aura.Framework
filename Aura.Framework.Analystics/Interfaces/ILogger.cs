namespace Aura.Framework.Analystics.Interfaces
{
    /// <summary>
    /// An interface for the logger class.
    /// </summary>
    public interface ILogger
    {
        #region Methods

        /// <summary>
        /// Reads and returns a <see cref="string"/> array value which contains all the lines of the log file.
        /// </summary>
        string[] Read();

        /// <summary>
        /// Reads and returns a <see cref="string"/> array value which contains all the lines of the log file that has any similarities to the parameter.
        /// </summary>
        string[] Read(string simularities);

        #endregion Methods
    }
}