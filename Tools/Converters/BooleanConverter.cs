using Aura.Framework.Tools.Converters.Interfaces;

namespace Aura.Framework.Tools.Converters
{
    /// <summary>
    /// A class that converts a <see cref="bool"/> value to an <see cref="int"/> value.
    /// </summary>
    public class BooleanConverter : IConverter<int, bool>, IConverter<string, bool>, IConverter<long, bool>, System.IDisposable
    {
        #region Methods

        /// <summary>
        /// Converts an <see cref="int"/> value into an <see cref="bool"/> value. 0 is equal to false and 1 or above is equal to true.
        /// </summary>
        public bool Convert(int t)
        {
            return (t == 0) ? false : true;
        }

        /// <summary>
        /// Converts a <see cref="string"/> value into a <see cref="bool"/> value.
        /// </summary>
        public bool Convert(string t)
        {
            try { return System.Convert.ToBoolean(t); }
            catch { return false; }
        }

        /// <summary>
        /// Converts a <see cref="long"/> value into a <see cref="bool"/> value.
        /// </summary>
        public bool Convert(long t)
        {
            try { return System.Convert.ToBoolean(t); }
            catch { return false; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() { }

        #endregion Methods
    }
}