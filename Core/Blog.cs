using System;

namespace Bloog
{
    public class Blog
    {
        private int Id { get; set; }

        private string name;
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

        public delegate void NameChanged(object sender, PropertyChangedEventArgs<int, string> e);
        public event NameChanged OnNameChanged;

        public Blog(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        public Blog(int id, string name) : this(name)
        {
            Id = id;
        }

        public void ChangeName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentNullException(nameof(newName), "New name cannot be empty.");

            Name = newName;
        }
    }
}
