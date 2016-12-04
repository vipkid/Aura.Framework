using Aura.Framework.Configurations.Models;
using System;
using System.Collections.Generic;

namespace Aura.Framework.Configurations.IO
{
    /// <summary>
    /// Represents the base class of all elements that exist in a <see cref="Configuration"/> class.
    /// </summary>
    public abstract class ConfigurationElement
    {
        #region Fields

        /// <summary>
        /// Represents the name of this element.
        /// </summary>
        private string _Name;

        /// <summary>
        /// Gets or sets the comment of this element.
        /// </summary>
        private Comment? _Comment;

        /// <summary>
        /// Represents a list of comments above this element.
        /// </summary>
        internal List<Comment> _Comments;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the name of this element.
        /// </summary>
        public string Name
        {
            get { return _Name; }
        }

        /// <summary>
        /// Gets or sets the comment of this element.
        /// </summary>
        public Comment? Comment
        {
            get { return _Comment; }
            set { _Comment = value; }
        }

        /// <summary>
        /// Gets the list of comments above this element.
        /// </summary>
        public List<Comment> PreComments
        {
            get
            {
                if (_Comments == null)
                    _Comments = new List<Comment>();

                return _Comments;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationElement"/> class.
        /// </summary>
        internal ConfigurationElement(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            _Name = name;
        }

        #endregion Constructors
    }
}