using System;
using System.Threading.Tasks;

namespace Bloog
{
    public interface IUserRepository
    {
        void AddAsync(User user);
        Task<User> FindAsync(Guid Id);
    }
}