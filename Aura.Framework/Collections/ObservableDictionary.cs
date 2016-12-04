using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Aura.Framework.Collections
{
    /// <summary>
    /// Represents an Onbservable dictionary.
    /// </summary>
    public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Dictionary{TKey, TValue}"/> class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        public ObservableDictionary() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        public ObservableDictionary(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class uses the specified <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        public ObservableDictionary(IEqualityComparer<TKey> comparer) : base(comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class that contains elements copied from the specified <see cref="IDictionary{TKey, TValue}"/> and uses the default equality comparer for the key type.
        /// </summary>
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class that is empty, has the specified initial capacity, and uses the specified <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class that contains elements copied from the specified <see cref="IDictionary{TKey, TValue}"/> and uses the specified <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer)
        {
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when an item is added, removed, changed, moved, or the entire list is refreshed.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region TValue

        /// <summary>
        /// Represents a value of the <see cref="ObservableDictionary{TKey, TValue}"/>.
        /// </summary>
        public new TValue this[TKey key]
        {
            get
            {
                return base[key];
            }
            set
            {
                TValue oldValue;
                bool exist = TryGetValue(key, out oldValue);
                var oldItem = new KeyValuePair<TKey, TValue>(key, oldValue);
                base[key] = value;
                var newItem = new KeyValuePair<TKey, TValue>(key, value);
                if (exist)
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, Keys.ToList().IndexOf(key)));
                }
                else
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem, Keys.ToList().IndexOf(key)));
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
                }
            }
        }

        #endregion TValue

        #region Methods

        /// <summary>
        /// Adds an object to the end of the <see cref="ObservableDictionary{TKey, TValue}"/>.
        /// </summary>
        public new void Add(TKey key, TValue value)
        {
            if (!ContainsKey(key))
            {
                var item = new KeyValuePair<TKey, TValue>(key, value);
                base.Add(key, value);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, Keys.ToList().IndexOf(key)));
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            } 
        }

        /// <summary>
        /// Removes an object from the <see cref="ObservableDictionary{TKey, TValue}"/>.
        /// </summary>
        public new bool Remove(TKey key)
        {
            TValue value;
            if (TryGetValue(key, out value))
            {
                var item = new KeyValuePair<TKey, TValue>(key, base[key]);
                bool result = base.Remove(key);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, Keys.ToList().IndexOf(key)));
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
                return result;
            }
            return false;
        }

        /// <summary>
        /// Removes all elements from the <see cref="ObservableDictionary{TKey, TValue}"/>.
        /// </summary>
        public new void Clear()
        {
            base.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
        }

        #endregion Methods

        #region Event Handlers

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        #endregion Event Handlers
    }
}