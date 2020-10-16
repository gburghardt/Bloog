using System;

namespace Bloog
{
    public class User
    {
        public Guid Id { get; }
        public string Username { get; }

        public User(string username) : this(Guid.NewGuid(), username)
        {
        }

        public User(Guid id, string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));

            Id = id;
            Username = username;
        }
    }
}
