using Bloog.Infrastructure;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bloog.SqlServer
{
    internal class BlogGateway : IBlogGateway
    {
        private const string InsertStatement = @"INSERT INTO [dbo].[Blog] OUTPUT Inserted.Id VALUES (@Name, @CreatedBy, @CreatedOn)";
        private const string SelectByIdStatement = @"SELECT [Name], [Id] FROM [dbo].[Blog] WHERE [Id] = @Id";

        private IAuditor Auditor { get; }
        private string ConnectionString { get; }

        private readonly SqlCommandFactory commandFactory;

        internal BlogGateway(IAuditor auditor, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            Auditor = auditor ?? throw new ArgumentNullException(nameof(auditor));
            ConnectionString = connectionString;
            commandFactory = new SqlCommandFactory(auditor);
        }

        public async Task<int> CreateBlogAsync(string name)
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(InsertStatement, connection))
            {
                await connection.OpenAsync();
                command.Parameters.AddWithValue("Name", name);
                command.Parameters.AddWithValue("Creator", Auditor.UserId);
                command.Parameters.AddWithValue("CreatedOn", Auditor.Now());

                return (int)await command.ExecuteScalarAsync();
            }
        }

        public async Task<Blog> FindBlogAsync(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
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

        private Blog MapToBlog(IDataReader reader, string columnNamePrefix = "")
        {
            var blog = new Blog(reader.GetInt32(columnNamePrefix + "Id"), reader[columnNamePrefix + "Name"].ToString());

            return blog;
        }

        public async Task SaveChangesAsync(Dictionary<int, Dictionary<string, PropertyChange>> updates)
        {
            if (updates.Count == 0)
                return;

            using (var connection = new SqlConnection(ConnectionString))
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
