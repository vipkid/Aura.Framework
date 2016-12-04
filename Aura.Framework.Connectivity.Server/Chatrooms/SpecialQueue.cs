using System.Collections.Generic;

namespace Aura.Framework.Connectivity.Server.Chatrooms
{
    /// <summary>
    /// Represents a queue implemented with a remove and contains method.
    /// </summary>
    public class SpecialQueue<T>
    {
        #region Fields

        /// <summary>
        /// Gets the number of elements contained in the <see cref="Queue{T}"/>.
        /// </summary>
        public int Count { get { return _List.Count; } }

        /// <summary>
        /// Represents a list to hold the items of the current instance of the <see cref="SpecialQueue{T}"/> class.
        /// </summary>
        private LinkedList<T> _List = new LinkedList<T>();

        #endregion Fields

        #region Methods

        /// <summary>
        /// Adds a new object to the queue list.
        /// </summary>
        public void Enqueue(T t)
        {
            _List.AddLast(t);
        }

        /// <summary>
        /// Returns and removes the object at the beginning of the <see cref="Queue{T}"/>.
        /// </summary>
        public T Dequeue()
        {
            var result = _List.First.Value;
            _List.RemoveFirst();
            return result;
        }

        /// <summary>
        /// Returns the object at the beginning of the <see cref="Queue{T}"/> without removing it.
        /// </summary>
        public T Peek()
        {
            return _List.First.Value;
        }

        /// <summary>
        /// Removes the object from the <see cref="Queue{T}"/>.
        /// </summary>
        public bool Remove(T t)
        {
            return _List.Remove(t);
        }

        /// <summary>
        /// Determines whether an element is in the <see cref="Queue{T}"/>.
        /// </summary>
        public bool Contains(T t)
        {
            return _List.Contains(t);
        }

        #endregion Methods
    }
}