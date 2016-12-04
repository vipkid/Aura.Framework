using Aura.Framework.Configurations.Exceptions;
using Aura.Framework.Configurations.IO;
using System;

namespace Aura.Framework.Configurations.Models
{
    /// <summary>
    /// Represents a setting in a <see cref="Configuration"/>.
    /// Settings are always stored in a <see cref="Section"/>.
    /// </summary>
    public sealed class Setting : ConfigurationElement
    {
        #region Fields

        private string mRawValue = string.Empty;
        private int mCachedArraySize = 0;
        private bool mShouldCalculateArraySize = false;
        private char mCachedArrayElementSeparator;

        #endregion Fields

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="Setting"/> class.
        /// </summary>
        public Setting(string name) : this(name, string.Empty)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Setting"/> class.
        /// </summary>
        public Setting(string name, object value)
            : base(name)
        {
            SetValue(value);
            mCachedArrayElementSeparator = Configuration.ArrayElementSeparator;
        }

        #endregion Construction

        #region Properties

        /// <summary>
        /// Gets or sets the value of this setting as a string.
        /// </summary>
        public string StringValue
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the value of this setting as a string array.
        /// </summary>
        public string[] StringValueArray
        {
            get { return GetValueArray<string>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the value of this setting as an int.
        /// </summary>
        public int IntValue
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the value of this setting as an int array.
        /// </summary>
        public int[] IntValueArray
        {
            get { return GetValueArray<int>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the value of this setting as a float.
        /// </summary>
        public float FloatValue
        {
            get { return GetValue<float>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the value of this setting as a float array.
        /// </summary>
        public float[] FloatValueArray
        {
            get { return GetValueArray<float>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the value of this setting as a double.
        /// </summary>
        public double DoubleValue
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the value of this setting as a double array.
        /// </summary>
        public double[] DoubleValueArray
        {
            get { return GetValueArray<double>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the value of this setting as a bool.
        /// </summary>
        public bool BoolValue
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the value of this setting as a bool array.
        /// </summary>
        public bool[] BoolValueArray
        {
            get { return GetValueArray<bool>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the value of this settings as a <see cref="DateTime"/>.
        /// </summary>
        public DateTime DateTimeValue
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the value of this setting as a <see cref="DateTime"/> array.
        /// </summary>
        public DateTime[] DateTimeValueArray
        {
            get { return GetValueArray<DateTime>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets a value indicating whether this setting is an array.
        /// </summary>
        public bool IsArray
        {
            get { return (ArraySize >= 0); }
        }

        /// <summary>
        /// Gets the size of the array that this setting represents. If this setting is not an array, -1 is returned.
        /// </summary>
        public int ArraySize
        {
            get
            {
                if (mCachedArrayElementSeparator != Configuration.ArrayElementSeparator)
                {
                    mCachedArrayElementSeparator = Configuration.ArrayElementSeparator;
                    mShouldCalculateArraySize = true;
                }

                if (mShouldCalculateArraySize)
                {
                    mCachedArraySize = CalculateArraySize();
                    mShouldCalculateArraySize = false;
                }

                return mCachedArraySize;
            }
        }

        private int CalculateArraySize()
        {
            if (string.IsNullOrEmpty(mRawValue))
                return -1;

            int arrayStartIdx = mRawValue.IndexOf('{');
            int arrayEndIdx = mRawValue.LastIndexOf('}');

            if (arrayStartIdx < 0 || arrayEndIdx < 0)
                return -1;

            for (int i = 0; i < arrayStartIdx; ++i)
                if (mRawValue[i] != ' ')
                    return -1;
            for (int i = arrayEndIdx + 1; i < mRawValue.Length; ++i)
                if (mRawValue[i] != ' ')
                    return -1;

            int arraySize = 0;

            for (int i = 0; i < mRawValue.Length; ++i)
                if (mRawValue[i] == Configuration.ArrayElementSeparator)
                    ++arraySize;

            if (arraySize == 0)
            {
                for (int i = arrayStartIdx + 1; i < arrayEndIdx; ++i)
                {
                    if (mRawValue[i] != ' ')
                    {
                        ++arraySize;
                        break;
                    }
                }
            }
            else if (arraySize > 0)
            {
                ++arraySize;
            }

            return arraySize;
        }

        #endregion Properties

        #region GetValue

        /// <summary>
        /// Gets this setting's value as a specific type.
        /// </summary>
        public object GetValue(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (type.IsArray)
                throw new InvalidOperationException("To obtain an array value, use GetValueArray() instead of GetValue().");

            if (this.IsArray)
                throw new InvalidOperationException("The setting represents an array. Use GetValueArray() to obtain its value.");

            return CreateObjectFromString(mRawValue, type);
        }

        /// <summary>
        /// Gets this setting's value as an array of a specific type.
        /// </summary>
        public object[] GetValueArray(Type elementType)
        {
            if (elementType.IsArray)
                throw CreateJaggedArraysNotSupportedEx(elementType);

            int myArraySize = this.ArraySize;
            if (ArraySize < 0)
                return null;

            var values = new object[myArraySize];

            if (myArraySize > 0)
            {
                var enumerator = new SettingArrayEnumerator(mRawValue);
                while (enumerator.Next())
                    values[enumerator.Index] = CreateObjectFromString(enumerator.Current, elementType);
            }

            return values;
        }

        /// <summary>
        /// Gets this setting's value as a specific type.
        /// </summary>
        public T GetValue<T>()
        {
            var type = typeof(T);

            if (type.IsArray)
                throw new InvalidOperationException("To obtain an array value, use GetValueArray() instead of GetValue().");

            if (this.IsArray)
                throw new InvalidOperationException("The setting represents an array. Use GetValueArray() to obtain its value.");

            return (T)CreateObjectFromString(mRawValue, type);
        }

        /// <summary>
        /// Gets this setting's value as an array of a specific type.
        /// </summary>
        public T[] GetValueArray<T>()
        {
            var type = typeof(T);

            if (type.IsArray)
                throw CreateJaggedArraysNotSupportedEx(type);

            int myArraySize = this.ArraySize;
            if (myArraySize < 0)
                return null;

            var values = new T[myArraySize];

            if (myArraySize > 0)
            {
                var enumerator = new SettingArrayEnumerator(mRawValue);
                while (enumerator.Next())
                    values[enumerator.Index] = (T)CreateObjectFromString(enumerator.Current, type);
            }

            return values;
        }

        private static object CreateObjectFromString(string value, Type dstType)
        {
            var underlyingType = Nullable.GetUnderlyingType(dstType);
            if (underlyingType != null)
            {
                if (string.IsNullOrEmpty(value))
                    return null;
                dstType = underlyingType;
            }

            var converter = Configuration.FindTypeStringConverter(dstType);
            if (converter == Configuration.FallbackConverter)
            {
                throw ConfigurationException.CreateOnConverterMissing(value, dstType);
            }

            try
            {
                return converter.ConvertFromString(value, dstType);
            }
            catch (Exception ex)
            {
                throw ConfigurationException.Create(value, dstType, ex);
            }
        }

        #endregion GetValue

        #region SetValue

        /// <summary>
        /// Sets the value of this setting via an object.
        /// </summary>
        public void SetValue(object value)
        {
            if (value == null)
            {
                SetEmptyValue();
                return;
            }

            var type = value.GetType();
            if (type.IsArray)
            {
                if (type.GetElementType().IsArray)
                    throw CreateJaggedArraysNotSupportedEx(type.GetElementType());

                var values = value as Array;
                var strings = new string[values.Length];

                for (int i = 0; i < values.Length; i++)
                {
                    object elemValue = values.GetValue(i);
                    var converter = Configuration.FindTypeStringConverter(elemValue.GetType());
                    strings[i] = converter.ConvertToString(elemValue);
                }

                mRawValue = string.Format("{{{0}}}", string.Join(Configuration.ArrayElementSeparator.ToString(), strings));
                mCachedArraySize = values.Length;
                mShouldCalculateArraySize = false;
            }
            else
            {
                var converter = Configuration.FindTypeStringConverter(type);
                mRawValue = converter.ConvertToString(value);
                mShouldCalculateArraySize = true;
            }
        }

        private void SetEmptyValue()
        {
            mRawValue = string.Empty;
            mCachedArraySize = 0;
            mShouldCalculateArraySize = false;
        }

        #endregion SetValue

        #region Public Methods

        /// <summary>
        /// Gets the string representation of the setting, without its comments.
        /// </summary>
        public override string ToString()
        {
            return ToString(false);
        }

        /// <summary>
        /// Gets the string representation of the setting.
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
                    return string.Format("{0}{1}{2} = {3} {4}",
                        string.Join(Environment.NewLine, preCommentStrings),
                        Environment.NewLine,
                        Name,
                        mRawValue,
                        Comment.ToString()
                        );
                }
                else if (Comment != null)
                {
                    return string.Format("{0} = {1} {2}", Name, mRawValue, Comment.ToString());
                }
                else if (hasPreComments)
                {
                    return string.Format("{0}{1}{2} = {3}",
                        string.Join(Environment.NewLine, preCommentStrings),
                        Environment.NewLine,
                        Name,
                        mRawValue
                        );
                }
            }

            return string.Format("{0} = {1}", Name, mRawValue);
        }

        #endregion Public Methods

        #region Exceptions

        private static ArgumentException CreateJaggedArraysNotSupportedEx(Type type)
        {
            Type elementType = type.GetElementType();
            while (elementType.IsArray)
                elementType = elementType.GetElementType();

            throw new ArgumentException(string.Format(
                "Jagged arrays are not supported. The type you have specified is '{0}', but '{1}' was expected.",
                type.Name, elementType.Name
                ));
        }

        #endregion Exceptions
    }
}