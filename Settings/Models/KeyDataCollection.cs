using System;
using System.Collections;
using System.Collections.Generic;

namespace Aura.Framework.Settings.Models
{
    /// <summary>
    /// Represents a collection of key data.
    /// </summary>
    public class KeyDataCollection : ICloneable, IEnumerable<KeyData>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyDataCollection"/> class.
        /// </summary>
        public KeyDataCollection() : this(EqualityComparer<string>.Default)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyDataCollection"/> class with a given search comparer.
        /// </summary>
        public KeyDataCollection(IEqualityComparer<string> searchComparer)
        {
            _SearchComparer = searchComparer;
            _KeyData = new Dictionary<string, KeyData>(_SearchComparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyDataCollection"/> class from a previous instance of <see cref="KeyDataCollection"/>.
        /// </summary>
        public KeyDataCollection(KeyDataCollection ori, IEqualityComparer<string> searchComparer) : this(searchComparer)
        {
            foreach (KeyData key in ori)
            {
                if (_KeyData.ContainsKey(key.KeyName))
                {
                    _KeyData[key.KeyName] = (KeyData)key.Clone();
                }
                else
                {
                    _KeyData.Add(key.KeyName, (KeyData)key.Clone());
                }
            }
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the value of a concrete key.
        /// </summary>
        public string this[string keyName]
        {
            get
            {
                if (_KeyData.ContainsKey(keyName))
                    return _KeyData[keyName].Value;

                return null;
            }
            set
            {
                if (!_KeyData.ContainsKey(keyName))
                {
                    AddKey(keyName);
                }

                _KeyData[keyName].Value = value;
            }
        }

        /// <summary>
        /// Return the number of keys in the collection.
        /// </summary>
        public int Count
        {
            get { return _KeyData.Count; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds a new key with the specified name and empty value and comments.
        /// </summary>
        public bool AddKey(string keyName)
        {
            if (!_KeyData.ContainsKey(keyName))
            {
                _KeyData.Add(keyName, new KeyData(keyName));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds a new key with the specified name and empty value and comments.
        /// </summary>
        [Obsolete("Pottentially buggy method! Use AddKey(KeyData keyData) instead (See comments in code for an explanation of the bug)")]
        public bool AddKey(string keyName, KeyData keyData)
        {
            // BUG: this actually can allow you to add the keyData having
            // keyData.KeyName different from the argument 'keyName' in this method
            // which doesn't make any sense
            if (AddKey(keyName))
            {
                _KeyData[keyName] = keyData;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a new key to the collection.
        /// </summary>
        public bool AddKey(KeyData keyData)
        {
            if (AddKey(keyData.KeyName))
            {
                _KeyData[keyData.KeyName] = keyData;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds a new key with the specified name and value to the collection.
        /// </summary>
        public bool AddKey(string keyName, string keyValue)
        {
            if (AddKey(keyName))
            {
                _KeyData[keyName].Value = keyValue;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Clears all comments of this section.
        /// </summary>
        public void ClearComments()
        {
            foreach (var keydata in this)
            {
                keydata.Comments.Clear();
            }
        }

        /// <summary>
        /// Gets if a specifyed key name exists in the collection.
        /// </summary>
        public bool ContainsKey(string keyName)
        {
            return _KeyData.ContainsKey(keyName);
        }

        /// <summary>
        /// Retrieves the data for a specified key given its name.
        /// </summary>
        public KeyData GetKeyData(string keyName)
        {
            if (_KeyData.ContainsKey(keyName))
                return _KeyData[keyName];
            return null;
        }

        /// <summary>
        /// Merges two instances of the <see cref="KeyDataCollection"/> class.
        /// </summary>
        public void Merge(KeyDataCollection keyDataToMerge)
        {
            foreach (var keyData in keyDataToMerge)
            {
                AddKey(keyData.KeyName);
                GetKeyData(keyData.KeyName).Comments.AddRange(keyData.Comments);
                this[keyData.KeyName] = keyData.Value;
            }
        }

        /// <summary>
        /// Deletes all keys in this collection.
        /// </summary>
        public void RemoveAllKeys()
        {
            _KeyData.Clear();
        }

        /// <summary>
        /// Deletes a previously existing key, including its associated data.
        /// </summary>
        public bool RemoveKey(string keyName)
        {
            return _KeyData.Remove(keyName);
        }

        /// <summary>
        /// Sets the key data associated to a specified key.
        /// </summary>
        public void SetKeyData(KeyData data)
        {
            if (data == null) return;

            if (_KeyData.ContainsKey(data.KeyName))
                RemoveKey(data.KeyName);

            AddKey(data);
        }

        #endregion Methods

        #region IEnumerable<KeyData> Members

        /// <summary>
        /// Allows iteration througt the collection.
        /// </summary>
        /// <returns>A strong-typed IEnumerator </returns>
        public IEnumerator<KeyData> GetEnumerator()
        {
            foreach (string key in _KeyData.Keys)
                yield return _KeyData[key];
        }

        #region IEnumerable Members

        /// <summary>
        /// Implementation needed
        /// </summary>
        /// <returns>A weak-typed IEnumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _KeyData.GetEnumerator();
        }

        #endregion IEnumerable Members

        #endregion IEnumerable<KeyData> Members

        #region ICloneable Members

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            return new KeyDataCollection(this, _SearchComparer);
        }

        #endregion ICloneable Members

        #region Non-public Members

        /// <summary>
        /// Hack for getting the last key value (if exists) w/out using LINQ and maintain support for earlier versions of .NET
        /// </summary>
        internal KeyData GetLast()
        {
            KeyData result = null;
            if (_KeyData.Keys.Count <= 0) return result;

            foreach (var k in _KeyData.Keys) result = _KeyData[k];
            return result;
        }

        /// <summary>
        /// Collection of KeyData for a given section.
        /// </summary>
        private readonly Dictionary<string, KeyData> _KeyData;

        /// <summary>
        /// Collection of KeyData for a given section.
        /// </summary>
        private IEqualityComparer<string> _SearchComparer;

        #endregion Non-public Members
    }
}