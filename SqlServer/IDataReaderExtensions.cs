using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Bloog.SqlServer
{
    static class IDataReaderExtensions
    {
        internal static Guid GetGuid(this IDataReader reader, string columnName)
        {
            return reader.GetGuid(reader.GetOrdinal(columnName));
        }

        internal static int GetInt32(this IDataReader reader, string columnName)
        {
            return reader.GetInt32(reader.GetOrdinal(columnName));
        }

        internal static string GetString(this IDataReader reader, string columnName)
        {
            return reader.GetString(reader.GetOrdinal(columnName));
        }
    }
}
