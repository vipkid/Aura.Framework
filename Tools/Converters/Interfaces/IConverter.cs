namespace Aura.Framework.Tools.Converters.Interfaces
{
    /// <summary>
    /// An interface for convert-related classes.
    /// </summary>
    public interface IConverter<TKey, TValue>
    {
        #region Methods

        /// <summary>
        /// Converts an object to another instance of an object.
        /// </summary>
        TValue Convert(TKey t);

        #endregion Methods
    }
}