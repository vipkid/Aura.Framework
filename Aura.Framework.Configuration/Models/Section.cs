using Aura.Framework.Configurations.Attributes;
using Aura.Framework.Configurations.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Aura.Framework.Configurations.Models
{
    /// <summary>
    /// Represents a group of <see cref="Setting"/> objects.
    /// </summary>
    public sealed class Section : ConfigurationElement, IEnumerable<Setting>
    {
        #region Fields

        private List<Setting> _Settings;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Creates a new instance of the <see cref="Section"/> class that is
        /// based on an existing object.
        /// Important: the section is built only from the public getter properties
        /// and fields of its type.
        /// When this method is called, all of those properties will be called
        /// and fields accessed once to obtain their values.
        /// Properties and fields that are marked with the <see cref="ConfigurationExcludeAttribute"/> attribute
        /// or are of a type that is marked with that attribute, are ignored.
        /// </summary>
        public static Section FromObject(string name, object obj)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("The section name must not be null or empty.", "name");

            if (obj == null)
                throw new ArgumentNullException("obj", "obj must not be null.");

            var section = new Section(name);
            var type = obj.GetType();

            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!prop.CanRead || ShouldIgnoreMappingFor(prop))
                {
                    continue;
                }

                Setting setting = new Setting(prop.Name, prop.GetValue(obj, null));
                section._Settings.Add(setting);
            }

            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if (ShouldIgnoreMappingFor(field))
                {
                    continue;
                }

                Setting setting = new Setting(field.Name, field.GetValue(obj));
                section._Settings.Add(setting);
            }

            return section;
        }

        /// <summary>
        /// Creates an object of a specific type, and maps the settings
        /// in this section to the public properties and writable fields of the object.
        /// Properties and fields that are marked with the <see cref="ConfigurationExcludeAttribute"/> attribute
        /// or are of a type that is marked with that attribute, are ignored.
        /// </summary>
        public T ToObject<T>() where T : new()
        {
            var obj = Activator.CreateInstance<T>();
            SetValuesTo(obj);
            return obj;
        }

        /// <summary>
        /// Creates an object of a specific type, and maps the settings
        /// in this section to the public properties and writable fields of the object.
        /// Properties and fields that are marked with the <see cref="ConfigurationExcludeAttribute"/> attribute
        /// or are of a type that is marked with that attribute, are ignored.
        /// </summary>
        public object ToObject(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(type.Name);

            var obj = Activator.CreateInstance(type);
            SetValuesTo(obj);
            return obj;
        }

        /// <summary>
        /// Assigns the values of an object's public properties and fields to the corresponding
        /// <b>already existing</b> settings in this section.
        /// Properties and fields that are marked with the <see cref="ConfigurationExcludeAttribute"/> attribute
        /// or are of a type that is marked with that attribute, are ignored.
        /// </summary>
        public void GetValuesFrom(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            var type = obj.GetType();

            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!prop.CanRead || ShouldIgnoreMappingFor(prop))
                    continue;

                var setting = FindSetting(prop.Name);
                if (setting != null)
                {
                    object value = prop.GetValue(obj, null);
                    setting.SetValue(value);
                }
            }

            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if (ShouldIgnoreMappingFor(field))
                    continue;

                var setting = FindSetting(field.Name);
                if (setting != null)
                {
                    object value = field.GetValue(obj);
                    setting.SetValue(value);
                }
            }
        }

        /// <summary>
        /// Assigns the values of this section to an object's public properties and fields.
        /// Properties and fields that are marked with the <see cref="ConfigurationExcludeAttribute"/> attribute
        /// or are of a type that is marked with that attribute, are ignored.
        /// </summary>
        public void SetValuesTo(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            var type = obj.GetType();

            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!prop.CanWrite || ShouldIgnoreMappingFor(prop))
                    continue;

                var setting = FindSetting(prop.Name);
                if (setting == null)
                    continue;

                object value = setting.GetValue(prop.PropertyType);
                if (value is Array)
                {
                    var settingArray = value as Array;
                    var propArray = prop.GetValue(obj, null) as Array;
                    if (propArray == null || propArray.Length != settingArray.Length)
                    {
                        propArray = Array.CreateInstance(prop.PropertyType.GetElementType(), settingArray.Length);
                    }

                    for (int i = 0; i < settingArray.Length; i++)
                        propArray.SetValue(settingArray.GetValue(i), i);

                    prop.SetValue(obj, propArray, null);
                }
                else
                {
                    prop.SetValue(obj, value, null);
                }
            }

            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if (field.IsInitOnly || ShouldIgnoreMappingFor(field))
                    continue;

                var setting = FindSetting(field.Name);
                if (setting == null)
                    continue;

                object value = setting.GetValue(field.FieldType);
                if (value is Array)
                {
                    var settingArray = value as Array;
                    var fieldArray = field.GetValue(obj) as Array;
                    if (fieldArray == null || fieldArray.Length != settingArray.Length)
                    {
                        fieldArray = Array.CreateInstance(field.FieldType.GetElementType(), settingArray.Length);
                    }

                    for (int i = 0; i < settingArray.Length; i++)
                        fieldArray.SetValue(settingArray.GetValue(i), i);

                    field.SetValue(obj, fieldArray);
                }
                else
                {
                    field.SetValue(obj, value);
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="bool"/> value whether a fields should be ignored that contains the <see cref="ConfigurationExcludeAttribute"/>.
        /// </summary>
        private static bool ShouldIgnoreMappingFor(MemberInfo member)
        {
            if (member.GetCustomAttributes(typeof(ConfigurationExcludeAttribute), false).Length > 0)
            {
                return true;
            }
            else
            {
                var prop = member as PropertyInfo;
                if (prop != null)
                    return prop.PropertyType.GetCustomAttributes(typeof(ConfigurationExcludeAttribute), false).Length > 0;

                var field = member as FieldInfo;
                if (field != null)
                    return field.FieldType.GetCustomAttributes(typeof(ConfigurationExcludeAttribute), false).Length > 0;
            }

            return false;
        }

        /// <summary>
        /// Gets an enumerator that iterates through the section.
        /// </summary>
        public IEnumerator<Setting> GetEnumerator()
        {
            return _Settings.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator that iterates through the section.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Adds a setting to the section.
        /// </summary>
        public void Add(Setting setting)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");

            if (Contains(setting))
                throw new ArgumentException("The specified setting already exists in the section.");

            _Settings.Add(setting);
        }

        /// <summary>
        /// Removes a setting from the section by its name.
        /// If there are multiple settings with the same name, only the first setting is removed.
        /// To remove all settings that have the name name, use the RemoveAllNamed() method instead.
        /// </summary>
        public bool Remove(string settingName)
        {
            if (string.IsNullOrEmpty(settingName))
                throw new ArgumentNullException("settingName");

            return Remove(FindSetting(settingName));
        }

        /// <summary>
        /// Removes a setting from the section.
        /// </summary>
        public bool Remove(Setting setting)
        {
            return _Settings.Remove(setting);
        }

        /// <summary>
        /// Removes all settings that have a specific name.
        /// </summary>
        public void RemoveAllNamed(string settingName)
        {
            if (string.IsNullOrEmpty(settingName))
                throw new ArgumentNullException("settingName");

            while (Remove(settingName)) ;
        }

        /// <summary>
        /// Clears the section of all settings.
        /// </summary>
        public void Clear()
        {
            _Settings.Clear();
        }

        /// <summary>
        /// Determines whether a specified setting is contained in the section.
        /// </summary>
        public bool Contains(Setting setting)
        {
            return _Settings.Contains(setting);
        }

        /// <summary>
        /// Determines whether a specifically named setting is contained in the section.
        /// </summary>
        public bool Contains(string settingName)
        {
            return FindSetting(settingName) != null;
        }

        /// <summary>
        /// Gets the number of settings that are in the section.
        /// </summary>
        public int SettingCount
        {
            get { return _Settings.Count; }
        }

        /// <summary>
        /// Gets or sets a setting by index.
        /// </summary>
        public Setting this[int index]
        {
            get
            {
                if (index < 0 || index >= _Settings.Count)
                    throw new ArgumentOutOfRangeException("index");

                return _Settings[index];
            }
        }

        /// <summary>
        /// Gets or sets a setting by its name.
        /// If there are multiple settings with the same name, the first setting is returned.
        /// If you want to obtain all settings that have the same name, use the GetSettingsNamed() method instead.
        /// </summary>
        public Setting this[string name]
        {
            get
            {
                var setting = FindSetting(name);

                if (setting == null)
                {
                    setting = new Setting(name);
                    _Settings.Add(setting);
                }

                return setting;
            }
        }

        /// <summary>
        /// Gets all settings that have a specific name.
        /// </summary>
        public IEnumerable<Setting> GetSettingsNamed(string name)
        {
            var settings = new List<Setting>();

            foreach (var setting in _Settings)
            {
                if (string.Equals(setting.Name, name, StringComparison.OrdinalIgnoreCase))
                    settings.Add(setting);
            }

            return settings;
        }

        /// <summary>
        /// Finds a setting by its name.
        /// </summary>
        private Setting FindSetting(string name)
        {
            foreach (var setting in _Settings)
            {
                if (string.Equals(setting.Name, name, StringComparison.OrdinalIgnoreCase))
                    return setting;
            }

            return null;
        }

        #endregion Methods

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        public Section(string name) : base(name)
        {
            _Settings = new List<Setting>();
        }

        #endregion Constructors

        #region Overrides

        /// <summary>
        /// Gets the string representation of the section, without its comments.
        /// </summary>
        public override string ToString()
        {
            return ToString(false);
        }

        /// <summary>
        /// Gets the string representation of the section.
        /// </summary>
        public string ToString(bool includeComment)
        {
            if (includeComment)
            {
                bool hasPreComments = _Comments != null && _Comments.Count > 0;

                string[] preCommentStrings = hasPreComments ?
                    _Comments.ConvertAll(c => c.ToString()).ToArray() : null;

                if (Comment != null && hasPreComments)
                {
                    return string.Format("{0}{1}[{2}] {3}",
                        string.Join(Environment.NewLine, preCommentStrings),
                        Environment.NewLine,
                        Name,
                        Comment.ToString()
                        );
                }
                else if (Comment != null)
                {
                    return string.Format("[{0}] {1}", Name, Comment.ToString());
                }
                else if (hasPreComments)
                {
                    return string.Format("{0}{1}[{2}]",
                        string.Join(Environment.NewLine, preCommentStrings),
                        Environment.NewLine,
                        Name
                        );
                }
            }

            return string.Format("[{0}]", Name);
        }

        #endregion Overrides
    }
}