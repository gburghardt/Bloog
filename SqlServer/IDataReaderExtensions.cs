using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Bloog.SqlServer
{
    static class IDataReaderExtensions
    {
        internal static int GetInt32(this IDataReader reader, string columnName)
        {
            return Convert.ToInt32(reader[columnName]);
        }
    }
}
