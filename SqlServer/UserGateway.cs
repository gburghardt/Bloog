using Bloog.Infrastructure;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

namespace Bloog.SqlServer
{
    class UserGateway : BaseGateway, IUserGateway
    {
        private const string InsertStatement = "INSERT into [dbo].[User] VALUES (@Id, @Username, @CreateUserId, @CreateDate, NULL, NULL)";

        internal UserGateway(IAuditor auditor, string connectionString) : base(auditor, connectionString)
        {
        }

        public async Task CreateUserAsync(Guid id, string username)
        {
            using (var connection = await GetOpenConnectionAsync())
            using (var command = new SqlCommand(InsertStatement, connection))
            {
                command.Parameters.AddWithValue("Id", id);
                command.Parameters.AddWithValue("Username", username);
                command.Parameters.AddWithValue("CreateUserId", Auditor.UserId);
                command.Parameters.AddWithValue("CreateDate", Auditor.Now());
                await command.ExecuteNonQueryAsync();
            }
        }

        public Task<User> FindUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
