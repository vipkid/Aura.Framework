using System;
using System.Collections.Generic;

namespace Aura.Framework.Settings.Models
{
    /// <summary>
    /// Information associated to a key from a config file. Includes both the value and the comments associated to the key.
    /// </summary>
    public class KeyData : ICloneable
    {
        #region Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyData"/> class.
        /// </summary>
        public KeyData(string keyName)
        {
            if (string.IsNullOrEmpty(keyName))
                throw new ArgumentException("key name can not be empty");

            _comments = new List<string>();
            _value = string.Empty;
            _keyName = keyName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyData"/> class from a previous instance of <see cref="KeyData"/>.
        /// </summary>
        public KeyData(KeyData ori)
        {
            _value = ori._value;
            _keyName = ori._keyName;
            _comments = new List<string>(ori._comments);
        }

        #endregion Initialization

        #region Properties

        /// <summary>
        /// Gets or sets the comment list associated to this key.
        /// </summary>
        public List<string> Comments
        {
            get { return _comments; }
            set { _comments = new List<string>(value); }
        }

        /// <summary>
        /// Gets or sets the value associated to this key.
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Gets or sets the name of the key.
        /// </summary>
        public string KeyName
        {
            get
            {
                return _keyName;
            }

            set
            {
                if (value != string.Empty)
                    _keyName = value;
            }
        }

        #endregion Properties

        #region ICloneable Members

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        public object Clone()
        {
            return new KeyData(this);
        }

        #endregion ICloneable Members

        #region Non-public Members

        /// <summary>
        /// Represents a list with comment lines associated to this key
        /// </summary>
        private List<string> _comments;

        /// <summary>
        /// Represents a unique value associated to this key.
        /// </summary>
        private string _value;

        /// <summary>
        /// Represents the name of the current key.
        /// </summary>
        private string _keyName;

        #endregion Non-public Members
    }
}