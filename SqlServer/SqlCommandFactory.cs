using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("Bloog.SqlServer.Tests")]

namespace Bloog.SqlServer
{
    internal class SqlCommandFactory
    {
        internal SqlCommand CreateUpdateStatement(Dictionary<string, PropertyChange> changes, string tableName, string keyName, object keyValue)
        {
            var command = new SqlCommand();
            var sql = new StringBuilder($"UPDATE {tableName} SET ");

            foreach (var change in changes)
            {
                sql.Append($"[{change.Key}] = @{change.Key}");
                command.Parameters.AddWithValue("@" + change.Key, change.Value.NewValue);
            }

            sql.Append($" WHERE [{keyName}] = @{keyName}");
            command.CommandText = sql.ToString();
            command.Parameters.AddWithValue(keyName, keyValue);

            return command;
        }
    }
}
