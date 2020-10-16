using Bloog.Infrastructure;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bloog.SqlServer
{
    internal class BlogGateway : BaseGateway, IBlogGateway
    {
        private const string InsertStatement = @"INSERT INTO [dbo].[Blog] (@Id, @Name, @CreatedBy, @CreatedOn)";

        private const string SelectByIdStatement = @"SELECT [Name], [Id] FROM [dbo].[Blog] WHERE [Id] = @Id";

        private const string SelectByUsernameStatement = @"SELECT [Name], [Id]
                                                           FROM [dbo].[Blog]
                                                               JOIN [dbo].[User] ON [dbo].[User].[Id] = [dbo].[Blog].[CreateUserId]
                                                           WHERE [dbo].[User].[Username] = @Username";

        private readonly SqlCommandFactory commandFactory;

        internal BlogGateway(IAuditor auditor, string connectionString) : base(auditor, connectionString)
        {
            commandFactory = new SqlCommandFactory(auditor);
        }

        public async Task<int> CreateBlogAsync(Guid id, string name)
        {
            using (var connection = await GetOpenConnectionAsync())
            using (var command = new SqlCommand(InsertStatement, connection))
            {
                await connection.OpenAsync();
                command.Parameters.AddWithValue("Id", id);
                command.Parameters.AddWithValue("Name", name);
                command.Parameters.AddWithValue("Creator", Auditor.UserId);
                command.Parameters.AddWithValue("CreatedOn", Auditor.Now());

                return (int)await command.ExecuteScalarAsync();
            }
        }

        public async Task<Blog> FindBlogAsync(Guid id)
        {
            using (var connection = await GetOpenConnectionAsync())
            using (var command = new SqlCommand(SelectByIdStatement, connection))
            {
                await connection.OpenAsync();
                command.Parameters.Add(new SqlParameter("Id", id));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    return MapToBlog(reader);
                }
            }
        }

        public async Task<IEnumerable<Blog>> FindByUser(string username)
        {
            using (var connection = await GetOpenConnectionAsync())
            using (var command = new SqlCommand(SelectByUsernameStatement, connection))
            {
                var results = new List<Blog>();

                await connection.OpenAsync();
                command.Parameters.AddWithValue("Username", username);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        results.Add(MapToBlog(reader));
                    }
                }

                return results.AsReadOnly();
            }
        }

        private Blog MapToBlog(IDataReader reader, string columnNamePrefix = "")
        {
            var blog = new Blog(reader.GetGuid(columnNamePrefix + "Id"), reader.GetString(columnNamePrefix + "Name"));

            return blog;
        }

        public async Task SaveChangesAsync(Dictionary<Guid, Dictionary<string, PropertyChange>> updates)
        {
            if (updates.Count == 0)
                return;

            using (var connection = await GetOpenConnectionAsync())
            {
                await connection.OpenAsync();

                foreach (var update in updates)
                {
                    var command = commandFactory.CreateUpdateStatement(update.Value, "[dbo].[Blog]", "Id", update.Key);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
