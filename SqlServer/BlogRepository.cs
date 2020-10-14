using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bloog.SqlServer
{
    public class BlogRepository : IBlogRepository
    {
        private const string InsertStatement = @"INSERT INTO [dbo].[Blog] OUTPUT Inserted.Id VALUES (@Name, @CreatedBy, @CreatedOn)";
        private const string SelectByIdStatement = @"SELECT [Name] FROM [dbo].[Blog] WHERE [Id] = @Id";

        private readonly Dictionary<int, Dictionary<string, PropertyChange>> Updates = new Dictionary<int, Dictionary<string, PropertyChange>>();

        private IAuditor Auditor { get; }
        private string ConnectionString { get; }
        internal SqlCommandFactory SqlCommand { get; }

        public BlogRepository(IAuditor auditor, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            Auditor = auditor ?? throw new ArgumentNullException(nameof(auditor));
            ConnectionString = connectionString;
            SqlCommand = new SqlCommandFactory(auditor);
        }

        public async void AddAsync(Blog blog)
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(InsertStatement, connection))
            {
                await connection.OpenAsync();
                command.Parameters.AddWithValue("Name", blog.Name);
                command.Parameters.AddWithValue("Creator", Auditor.UserId);
                command.Parameters.AddWithValue("CreatedOn", Auditor.Now());

                int newId = (int)await command.ExecuteScalarAsync();

                blog.GetType().GetProperty("Id").SetValue(blog, newId, null);
            }
        }

        public async Task<Blog> FindAsync(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(SelectByIdStatement, connection))
            {
                await connection.OpenAsync();
                command.Parameters.Add(new SqlParameter("Id", id));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var blog = new Blog(id, reader["Name"].ToString());

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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    DiscardChanges();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BlogRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //private void Dispose(bool disposing)
        //{
        //    if (!disposing)
        //        return;

        //    DiscardChanges();
        //}
    }
}
