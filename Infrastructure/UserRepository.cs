using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bloog.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserGateway gateway;

        public UserRepository(IUserGateway gateway)
        {
            this.gateway = gateway;
        }

        public async void AddAsync(User user)
        {
            await gateway.CreateUserAsync(user.Id, user.Username);
        }

        public async Task<User> FindAsync(Guid id)
        {
            return await gateway.FindUserAsync(id);
        }
    }
}
