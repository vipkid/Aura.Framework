using Aura.Framework.Settings.Models.Configuration;

namespace Aura.Framework.Settings.Models.Formatting
{
    /// <summary>
    /// Formats a IniData structure to an string
    /// </summary>
    public interface DataFormatter
    {
        /// <summary>
        /// Produces an string given
        /// </summary>
        /// <returns>The data to string.</returns>
        /// <param name="iniData">Ini data.</param>
        string IniDataToString(Data iniData);

        /// <summary>
        /// Configuration used by this formatter when converting IniData
        /// to an string
        /// </summary>
        ParseConfiguration Configuration { get; set; }
    }
}