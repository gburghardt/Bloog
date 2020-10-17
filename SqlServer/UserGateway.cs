using Bloog.Infrastructure;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

namespace Bloog.SqlServer
{
    class UserGateway : BaseGateway, IUserGateway
    {
        private const string DeleteByIdStatement = "DELETE FROM [dbo].[User] WHERE [Id] = @Id";
        private const string InsertStatement = "INSERT into [dbo].[User] VALUES (@Id, @Username, @CreateUserId, @CreateDate, NULL, NULL)";
        private const string SelectByIdStatement = "SELECT [Id], [Username] FROM [dbo].[User] WHERE [Id] = @Id";

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

        public async Task<User> FindUserAsync(Guid id)
        {
            using (var connection = await GetOpenConnectionAsync())
            using (var command = new SqlCommand(SelectByIdStatement, connection))
            {
                command.Parameters.AddWithValue("Id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        return MapToUser(reader);
                    }

                    return null;
                }
            }
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            using (var connection = await GetOpenConnectionAsync())
            using (var command = new SqlCommand(DeleteByIdStatement, connection))
            {
                command.Parameters.AddWithValue("Id", id);

                var rowsAffected = await command.ExecuteNonQueryAsync();

                return rowsAffected > 0;
            }
        }

        private User MapToUser(SqlDataReader reader)
        {
            return new User(reader.GetGuid("Id"), reader.GetString("Username"));
        }
    }
}
