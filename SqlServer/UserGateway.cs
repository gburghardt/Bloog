using Bloog.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bloog.SqlServer
{
    class UserGateway : BaseGateway, IUserGateway
    {
        internal UserGateway(IAuditor auditor, string connectionString) : base(auditor, connectionString)
        {
        }

        public Task CreateUserAsync(Guid id, string username)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
