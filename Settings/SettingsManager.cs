using Aura.Framework.Settings;
using Aura.Framework.Settings.Models;
using Aura.Framework.Settings.Parser;
using System;
using System.Diagnostics;
using System.Text;

namespace Aura.Windows.Content.Options.Managers
{
    /// <summary>
    /// Provides functionality for reading, writing, creating and updating of settings.
    /// </summary>
    public class SettingsManager
    {
        #region Fields

        /// <summary>
        /// Represents the file location of the config file.
        /// </summary>
        public string FileLocation { get; private set; }

        /// <summary>
        /// Represents the type of encoding that is used to read/write files.
        /// </summary>
        public Encoding Encoding { get; private set; } = Encoding.ASCII;

        #endregion Fields

        #region Contructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsManager"/> class.
        /// </summary>
        public SettingsManager(string fileLocation)
        {
            FileLocation = fileLocation;

            if (!FileLocation.EndsWith(".ini"))
                throw new ArgumentException("Invalid file-name.", new Exception("The file name must end with .ini,"));
        }

        #endregion Contructors

        #region Methods

        /// <summary>
        /// Saves a <see cref="string"/> value under a certain key.
        /// </summary>
        public void Save(string section, string key, string value)
        {
            Data data = GetData(FileLocation);

            if (data == null)
                data = new Data();

            try
            {
                data[section][key] = value;
            }
            catch (NullReferenceException)
            {
                data.Merge(Create(section, key, value));
            }
            Save(data);
        }

        /// <summary>
        /// Saves a <see cref="bool"/> value under a certain key.
        /// </summary>
        public void Save(string section, string key, bool value)
        {
            Save(section, key, value.ToString());
        }

        /// <summary>
        /// Saves an <see cref="int"/> value under a certain key.
        /// </summary>
        public void Save(string section, string key, int value)
        {
            Save(section, key, value.ToString());
        }

        /// <summary>
        /// Merges an instance of the <see cref="SettingsManager"/> class with the current instance.
        /// </summary>
        public void Merge(SettingsManager manager)
        {
            Merge(manager.ToString());
        }

        /// <summary>
        /// Merges an instance of the <see cref="SettingsManager"/> class with the current instance.
        /// </summary>
        public void Merge(string file)
        {
            if (!file.EndsWith(".ini"))
                return;

            try
            {
                using (var parser = new SettingsDataParser())
                {
                    Data currentData = parser.Parse(FileLocation);
                    Data toMerge = parser.Parse(file);

                    currentData.Merge(toMerge);
                    Save(currentData);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine($"Failed to merge '{file}'.");
            }
        }

        /// <summary>
        /// Returns a <see cref="bool"/> value associated with the key.
        /// </summary>
        public bool GetBool(string section, string key)
        {
            try
            {
                Data data = GetData(FileLocation);

                string dataString = data[section][key];
                return bool.Parse(dataString);
            }
            catch (Exception)
            {
                Debug.WriteLine($"Failed to load a boolean setting from file ({section} - {key}).");
                return false;
            }
        }

        /// <summary>
        /// Returns a <see cref="string"/> value associated with the key.
        /// </summary>
        public string GetString(string section, string key)
        {
            try
            {
                Data data = GetData(FileLocation);

                return data[section][key];
            }
            catch (Exception)
            {
                Debug.WriteLine($"Failed to load a string setting from file ({section} - {key}).");
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns a <see cref="int"/> value associated with the key.
        /// </summary>
        public int GetInt(string section, string key)
        {
            try
            {
                Data data = GetData(FileLocation);

                string dataString = data[section][key];
                return int.Parse(dataString);
            }
            catch (Exception)
            {
                Debug.WriteLine($"Failed to load an int setting from file ({section} - {key}).");
                return 0;
            }
        }

        #endregion Methods

        #region Internal Methods

        /// <summary>
        /// Gets an instance of the <see cref="Data"/> class associated with the current instance of the <see cref="SettingsManager"/> class.
        /// </summary>
        protected internal Data GetData(string file)
        {
            try
            {
                using (DataParser parser = new DataParser())
                {
                    return parser.ReadFile(file, Encoding);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine($"Failed to load data setting from file ({file}).");
                return null;
            }
        }

        /// <summary>
        /// Creates an empty and new ini file.
        /// </summary>
        protected internal Data Create(string section, string key, string value)
        {
            try
            {
                Data data = new Data();

                data.Sections.AddSection(section);
                data.Sections.GetSectionData(section).Keys.AddKey(key, value);

                return data;
            }
            catch (Exception)
            {
                Debug.WriteLine($"Failed to save data object.");
                return null;
            }
        }

        /// <summary>
        /// Saves a <see cref="Data"/> object to the file and returns itself.
        /// </summary>
        protected internal Data Save(Data data)
        {
            try
            {
                using (DataParser parser = new DataParser())
                {
                    parser.WriteFile(FileLocation, data, Encoding);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine($"Failed to save data object.");
            }

            return data;
        }

        #endregion Internal Methods

        #region Overrides

        /// <summary>
        /// Returns a string that is equal to the settings file location.
        /// </summary>
        public override string ToString()
        {
            return FileLocation;
        }

        #endregion Overrides
    }
}