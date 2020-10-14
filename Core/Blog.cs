using System;

namespace Bloog
{
    /// <summary>
    /// Represents a collection of irreverant nonsense that spews forth from the frontal cortex of a Netizen.
    /// </summary>
    public class Blog
    {
        private int Id { get; set; }

        private string name;

        /// <summary>
        /// The name of this blog.
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                if (name == value)
                    return;

                var oldName = name;
                name = value;
                OnNameChanged?.Invoke(this, new PropertyChangedEventArgs<int, string>(Id, oldName, name));
            }
        }

        /// <summary>
        /// Event triggered when the name of this blog gets changed.
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<int, string>> OnNameChanged;

        /// <summary>
        /// Initializes a new blog.
        /// </summary>
        /// <param name="name">The name of this new blog.</param>
        public Blog(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        /// <summary>
        /// Initializes a new blog object representing an existing blog.
        /// </summary>
        /// <param name="id">Identifier for this blog.</param>
        /// <param name="name">Name of this blog.</param>
        public Blog(int id, string name) : this(name)
        {
            Id = id;
        }

        /// <summary>
        /// Changes the name of this blog.
        /// </summary>
        /// <param name="newName">The new, cooler name.</param>
        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentNullException(nameof(newName), "New name cannot be empty.");

            Name = newName;
        }
    }
}
