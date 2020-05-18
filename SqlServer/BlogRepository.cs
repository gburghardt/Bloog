using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bloog.SqlServer
{
    public class BlogRepository : IDisposable
    {
        private const string InsertStatement = @"INSERT INTO [dbo].[Blog] OUTPUT Inserted.Id VALUES (@Name)";
        private const string SelectByIdStatement = @"SELECT [Name] FROM [dbo].[Blog] WHERE [Id] = @Id";

        private readonly Dictionary<int, Dictionary<string, PropertyChange>> Updates = new Dictionary<int, Dictionary<string, PropertyChange>>();
        private string ConnectionString { get; }
        internal SqlCommandFactory SqlCommand { get; }

        public BlogRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            ConnectionString = connectionString;
            SqlCommand = new SqlCommandFactory();
        }

        public async void Add(Blog blog)
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(InsertStatement, connection))
            {
                command.Parameters.AddWithValue("Name", blog.Name);

                int newId = (int)await command.ExecuteScalarAsync();

                blog.GetType().GetProperty("Id").SetValue(blog, newId, null);
            }
        }

        public async Task<Blog> Find(int blogId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(SelectByIdStatement, connection))
            {
                await connection.OpenAsync();
                command.Parameters.Add(new SqlParameter("Id", blogId));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var blog = new Blog(blogId, reader["Name"].ToString());

                    blog.OnNameChanged += HandleBlogNameChanged;

                    return blog;
                }
            }
        }

        private void HandleBlogNameChanged(object sender, PropertyChangedEventArgs<int, string> e)
        {
            if (!Updates.ContainsKey(e.Key))
                Updates[e.Key] = new Dictionary<string, PropertyChange>();

            Updates[e.Key]["Name"] = new PropertyChange(e.OldValue, e.NewValue);
        }

        public void DiscardChanges()
        {
            Updates.Clear();
        }

        public async void SaveChanges()
        {
            if (Updates.Count == 0)
                return;

            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                foreach (var update in Updates)
                {
                    var command = SqlCommand.CreateUpdateStatement(update.Value, "[dbo].[Blog]", "Id", update.Key);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            DiscardChanges();
        }
    }
}
