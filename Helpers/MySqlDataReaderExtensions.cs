using MySql.Data.MySqlClient;
using System;

namespace ClinicApp.Helpers
{
    public static class MySqlDataReaderExtensions
    {
        public static T SafeGet<T>(this MySqlDataReader dr, string columnName)
        {
            var value = dr[columnName];

            if (value == DBNull.Value || value == null)
                return default!;

            // Handle nullable types
            var targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            return (T)Convert.ChangeType(value, targetType);
        }
    }
}
