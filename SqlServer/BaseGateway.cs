using Bloog.Infrastructure;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bloog.SqlServer
{
    abstract class BaseGateway
    {
        protected IAuditor Auditor { get; }
        private string ConnectionString { get; }

        protected BaseGateway(IAuditor auditor, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            Auditor = auditor ?? throw new ArgumentNullException(nameof(auditor));
            ConnectionString = connectionString;
        }

        protected async Task<SqlConnection> GetOpenConnectionAsync()
        {
            var connection = new SqlConnection(ConnectionString);

            await connection.OpenAsync();

            return connection;
        }
    }
}
