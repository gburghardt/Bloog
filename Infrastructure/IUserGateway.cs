using System;
using System.Threading.Tasks;

namespace Bloog.Infrastructure
{
    public interface IUserGateway
    {
        Task CreateUserAsync(Guid id, string username);
        Task<User> FindUserAsync(Guid id);
        Task<bool> DeleteUserAsync(Guid id);
    }
}