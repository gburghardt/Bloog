using Bloog.Infrastructure;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Bloog.SqlServer.Tests")]

namespace Bloog.SqlServer
{
    internal class SqlCommandFactory
    {
        private const string UpdateStatementTemplate = "UPDATE {0} SET {1} WHERE [{2}] = @{2}";
        private readonly IAuditor auditor;

        public SqlCommandFactory(IAuditor auditor)
        {
            this.auditor = auditor ?? throw new ArgumentNullException(nameof(auditor));
        }

        internal SqlCommand CreateUpdateStatement(Dictionary<string, PropertyChange> changes, string tableName, string keyName, object keyValue)
        {
            if (changes.Count == 0)
                throw new InvalidOperationException("No changes detected");
            
            var command = new SqlCommand();
            var columns = new List<string>();

            foreach (var change in changes)
            {
                columns.Add($"[{change.Key}] = @{change.Key}");
                command.Parameters.AddWithValue(change.Key, change.Value.NewValue);
            }

            columns.Add("[UpdatedBy] = @UpdatedBy");
            command.Parameters.AddWithValue("UpdatedBy", auditor.UserId);
            columns.Add("[UpdatedOn] = @UpdatedOn");
            command.Parameters.AddWithValue("UpdatedOn", auditor.Now());
            command.Parameters.AddWithValue(keyName, keyValue);
            command.CommandText = string.Format(UpdateStatementTemplate, tableName, string.Join(", ", columns), keyName);

            return command;
        }
    }
}
