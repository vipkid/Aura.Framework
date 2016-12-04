using Aura.Framework.Configurations.Converters.TypeConverters;
using Aura.Framework.Configurations.Interfaces;
using Aura.Framework.Configurations.IO;
using Aura.Framework.Configurations.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Aura.Framework.Configurations
{
    /// <summary>
    /// Represents a configuration. Configurations contain one or multiple sections that in turn can contain one or multiple settings.
    /// The <see cref="Configuration"/> class is designed to work with classic configuration formats such as .ini and .cfg, but is not limited to these.
    /// </summary>
    public partial class Configuration : IEnumerable<Section>
    {
        #region Fields

        private static NumberFormatInfo _NumberFormat;
        private static DateTimeFormatInfo _DateTimeFormat;
        private static char[] _ValidCommentChars;
        private static char _ArrayElementSeparator;
        private static IConverter _FallbackConverter;
        private static Dictionary<Type, IConverter> _TypeStringConverters;

        internal List<Section> _Sections;

        #endregion Fields

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        static Configuration()
        {
            _NumberFormat = CultureInfo.InvariantCulture.NumberFormat;
            _DateTimeFormat = CultureInfo.InvariantCulture.DateTimeFormat;
            _ValidCommentChars = new[] { '#', ';', '\'' };
            _ArrayElementSeparator = ',';

            _FallbackConverter = new FallbackStringConverter();
            _TypeStringConverters = new Dictionary<Type, IConverter>()
            {
                { typeof(bool), new BoolStringConverter() },
                { typeof(byte), new ByteStringConverter() },
                { typeof(char), new CharStringConverter() },
                { typeof(DateTime), new DateTimeStringConverter() },
                { typeof(decimal), new DecimalStringConverter() },
                { typeof(double), new DoubleStringConverter() },
                { typeof(Enum), new EnumStringConverter() },
                { typeof(short), new Int16StringConverter() },
                { typeof(int), new Int32StringConverter() },
                { typeof(long), new Int64StringConverter() },
                { typeof(sbyte), new SByteStringConverter() },
                { typeof(float), new SingleStringConverter() },
                { typeof(string), new StringStringConverter() },
                { typeof(ushort), new UInt16StringConverter() },
                { typeof(uint), new UInt32StringConverter() },
                { typeof(ulong), new UInt64StringConverter() }
            };

            IgnoreInlineComments = false;
            IgnorePreComments = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            _Sections = new List<Section>();
        }

        #endregion Construction

        #region Public Methods

        /// <summary>
        /// Gets an enumerator that iterates through the configuration.
        /// </summary>
        public IEnumerator<Section> GetEnumerator()
        {
            return _Sections.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator that iterates through the configuration.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Adds a section to the configuration.
        /// </summary>
        public void Add(Section section)
        {
            if (section == null)
                throw new ArgumentNullException("section");

            if (Contains(section))
                throw new ArgumentException("The specified section already exists in the configuration.");

            _Sections.Add(section);
        }

        /// <summary>
        /// Removes a section from the configuration by its name.
        /// </summary>
        public bool Remove(string sectionName)
        {
            if (string.IsNullOrEmpty(sectionName))
                throw new ArgumentNullException("sectionName");

            return Remove(FindSection(sectionName));
        }

        /// <summary>
        /// Removes a section from the configuration.
        /// </summary>
        public bool Remove(Section section)
        {
            return _Sections.Remove(section);
        }

        /// <summary>
        /// Removes all sections that have a specific name.
        /// </summary>
        public void RemoveAllNamed(string sectionName)
        {
            if (string.IsNullOrEmpty(sectionName))
                throw new ArgumentNullException("sectionName");

            while (Remove(sectionName)) ;
        }

        /// <summary>
        /// Clears the configuration of all sections.
        /// </summary>
        public void Clear()
        {
            _Sections.Clear();
        }

        /// <summary>
        /// Determines whether a specified section is contained in the configuration.
        /// </summary>
        public bool Contains(Section section)
        {
            if (section == null)
                throw new ArgumentNullException("section");

            return _Sections.Contains(section);
        }

        /// <summary>
        /// Determines whether a specifically named section is contained in the configuration.
        /// </summary>
        public bool Contains(string sectionName)
        {
            if (sectionName == null)
                throw new ArgumentNullException("sectionName");

            return FindSection(sectionName) != null;
        }

        /// <summary>
        /// Determines whether a specifically named section is contained in the configuration and whether that section in turn contains a specifically named setting.
        /// </summary>
        public bool Contains(string sectionName, string settingName)
        {
            if (sectionName == null)
                throw new ArgumentNullException("sectionName");

            if (settingName == null)
                throw new ArgumentNullException("settingName");

            Section section = FindSection(sectionName);
            return section != null && section.Contains(settingName);
        }

        /// <summary>
        /// Registers a type converter to be used for setting value conversions.
        /// </summary>
        public static void RegisterTypeStringConverter(IConverter converter)
        {
            if (converter == null)
                throw new ArgumentNullException("converter");

            var type = converter.ConvertibleType;
            if (_TypeStringConverters.ContainsKey(type))
                throw new InvalidOperationException(string.Format("A converter for type '{0}' is already registered.", type.FullName));
            else
                _TypeStringConverters.Add(type, converter);
        }

        /// <summary>
        /// Deregisters a type converter from setting value conversion.
        /// </summary>
        public static void DeregisterTypeStringConverter(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (!_TypeStringConverters.ContainsKey(type))
                throw new InvalidOperationException(string.Format("No converter is registered for type '{0}'.", type.FullName));
            else
                _TypeStringConverters.Remove(type);
        }

        internal static IConverter FindTypeStringConverter(Type type)
        {
            IConverter converter = null;
            if (!_TypeStringConverters.TryGetValue(type, out converter))
                converter = _FallbackConverter;

            return converter;
        }

        internal static IConverter FallbackConverter
        {
            get { return _FallbackConverter; }
        }

        #endregion Public Methods

        #region Load

        /// <summary>
        /// Loads a configuration from a file auto-detecting the encoding and using the default parsing settings.
        /// </summary>
        public static Configuration LoadFromFile(string filename)
        {
            return LoadFromFile(filename, null);
        }

        /// <summary>
        /// Loads a configuration from a file.
        /// </summary>
        public static Configuration LoadFromFile(string filename, Encoding encoding)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("filename");

            if (!File.Exists(filename))
                throw new FileNotFoundException("Configuration file not found.", filename);

            return (encoding == null) ?
                LoadFromString(File.ReadAllText(filename)) :
                LoadFromString(File.ReadAllText(filename, encoding));
        }

        /// <summary>
        /// Loads a configuration from a text stream auto-detecting the encoding and using the default parsing settings.
        /// </summary>
        public static Configuration LoadFromStream(Stream stream)
        {
            return LoadFromStream(stream, null);
        }

        /// <summary>
        /// Loads a configuration from a text stream.
        /// </summary>
        public static Configuration LoadFromStream(Stream stream, Encoding encoding)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            string source = null;

            var reader = (encoding == null) ?
                new StreamReader(stream) :
                new StreamReader(stream, encoding);

            using (reader)
                source = reader.ReadToEnd();

            return LoadFromString(source);
        }

        /// <summary>
        /// Loads a configuration from text (source code).
        /// </summary>
        public static Configuration LoadFromString(string source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return ConfigurationReader.ReadFromString(source);
        }

        #endregion Load

        #region LoadBinary

        /// <summary>
        /// Loads a configuration from a binary file using the <b>default</b> <see cref="BinaryReader"/>.
        /// </summary>
        public static Configuration LoadFromBinaryFile(string filename)
        {
            return LoadFromBinaryFile(filename, null);
        }

        /// <summary>
        /// Loads a configuration from a binary file using a specific <see cref="BinaryReader"/>.
        /// </summary>
        public static Configuration LoadFromBinaryFile(string filename, BinaryReader reader)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("filename");

            using (var stream = File.OpenRead(filename))
                return LoadFromBinaryStream(stream, reader);
        }

        /// <summary>
        /// Loads a configuration from a binary stream, using the default <see cref="BinaryReader"/>.
        /// </summary>
        public static Configuration LoadFromBinaryStream(Stream stream)
        {
            return LoadFromBinaryStream(stream, null);
        }

        /// <summary>
        /// Loads a configuration from a binary stream, using a specific <see cref="BinaryReader"/>.
        /// </summary>
        public static Configuration LoadFromBinaryStream(Stream stream, BinaryReader reader)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            return ConfigurationReader.ReadFromBinaryStream(stream, reader);
        }

        #endregion LoadBinary

        #region Save

        /// <summary>
        /// Saves the configuration to a file using the default character encoding, which is UTF8.
        /// </summary>
        public void SaveToFile(string filename)
        {
            SaveToFile(filename, null);
        }

        /// <summary>
        /// Saves the configuration to a file.
        /// </summary>
        public void SaveToFile(string filename, Encoding encoding)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("filename");

            using (var stream = new FileStream(filename, FileMode.Create, FileAccess.Write))
                SaveToStream(stream, encoding);
        }

        /// <summary>
        /// Saves the configuration to a stream using the default character encoding, which is UTF8.
        /// </summary>
        public void SaveToStream(Stream stream)
        {
            SaveToStream(stream, null);
        }

        /// <summary>
        /// Saves the configuration to a stream.
        /// </summary>
        public void SaveToStream(Stream stream, Encoding encoding)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            ConfigurationWriter.WriteToStreamTextual(this, stream, encoding);
        }

        #endregion Save

        #region SaveBinary

        /// <summary>
        /// Saves the configuration to a binary file, using the default <see cref="BinaryWriter"/>.
        /// </summary>
        public void SaveToBinaryFile(string filename)
        {
            SaveToBinaryFile(filename, null);
        }

        /// <summary>
        /// Saves the configuration to a binary file, using a specific <see cref="BinaryWriter"/>.
        /// </summary>
        public void SaveToBinaryFile(string filename, BinaryWriter writer)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("filename");

            using (var stream = new FileStream(filename, FileMode.Create, FileAccess.Write))
                SaveToBinaryStream(stream, writer);
        }

        /// <summary>
        /// Saves the configuration to a binary stream, using the default <see cref="BinaryWriter"/>.
        /// </summary>
        public void SaveToBinaryStream(Stream stream)
        {
            SaveToBinaryStream(stream, null);
        }

        /// <summary>
        /// Saves the configuration to a binary file, using a specific <see cref="BinaryWriter"/>.
        /// </summary>
        public void SaveToBinaryStream(Stream stream, BinaryWriter writer)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            ConfigurationWriter.WriteToStreamBinary(this, stream, writer);
        }

        #endregion SaveBinary

        #region Properties

        /// <summary>
        /// Gets or sets the number format that is used for value conversion in SharpConfig. The default value is <see cref="CultureInfo.InvariantCulture.NumberFormat"/>.
        /// </summary>
        public static NumberFormatInfo NumberFormat
        {
            get { return _NumberFormat; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _NumberFormat = value;
            }
        }

        //// <summary>
        /// Gets or sets the date format that is used for value conversion in SharpConfig. The default value is <see cref="CultureInfo.InvariantCulture.DateTimeFormat"/>.
        /// </summary>
        public static DateTimeFormatInfo DateTimeFormat
        {
            get { return _DateTimeFormat; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _DateTimeFormat = value;
            }
        }

        /// <summary>
        /// Gets or sets the array that contains all comment delimiting characters.
        /// </summary>
        public static char[] ValidCommentChars
        {
            get { return _ValidCommentChars; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (value.Length == 0)
                {
                    throw new ArgumentException(
                        "The comment chars array must not be empty.",
                        "value");
                }

                _ValidCommentChars = value;
            }
        }

        /// <summary>
        /// Gets or sets the array element separator character for settings. The default value is ','. Remember that after you change this value while <see cref="Setting"/> instances exist, to expect their ArraySize and other array-related values to return different values.
        /// </summary>
        public static char ArrayElementSeparator
        {
            get { return _ArrayElementSeparator; }
            set
            {
                if (value == '\0')
                    throw new ArgumentException("Zero-character is not allowed.");

                _ArrayElementSeparator = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether inline-comments should be ignored when parsing a configuration.
        /// </summary>
        public static bool IgnoreInlineComments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether pre-comments should be ignored when parsing a configuration.
        /// </summary>
        public static bool IgnorePreComments { get; set; }

        /// <summary>
        /// Gets the number of sections that are in the configuration.
        /// </summary>
        public int SectionCount
        {
            get { return _Sections.Count; }
        }

        /// <summary>
        /// Gets or sets a section by index.
        /// </summary>
        public Section this[int index]
        {
            get
            {
                if (index < 0 || index >= _Sections.Count)
                    throw new ArgumentOutOfRangeException("index");

                return _Sections[index];
            }
        }

        /// <summary>
        /// Gets or sets a section by its name.
        /// </summary>
        public Section this[string name]
        {
            get
            {
                var section = FindSection(name);

                if (section == null)
                {
                    section = new Section(name);
                    Add(section);
                }

                return section;
            }
        }

        /// <summary>
        /// Gets all sections that have a specific name.
        /// </summary>
        public IEnumerable<Section> GetSectionsNamed(string name)
        {
            var sections = new List<Section>();

            foreach (var section in _Sections)
            {
                if (string.Equals(section.Name, name, StringComparison.OrdinalIgnoreCase))
                    sections.Add(section);
            }

            return sections;
        }

        /// <summary>
        /// Finds a section by its name.
        /// </summary>
        private Section FindSection(string name)
        {
            foreach (var section in _Sections)
            {
                if (string.Equals(section.Name, name, StringComparison.OrdinalIgnoreCase))
                    return section;
            }

            return null;
        }

        #endregion Properties
    }
}