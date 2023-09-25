using System.Data;
using System.Data.SqlClient;

namespace BingNew.ORM.Query
{
    public static class SqlExtensionScalar
    {

        public static dynamic? ExecuteScalar(this SqlConnection connection, string sql)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();
            using (var command = new SqlCommand(sql, connection))
            {
                var result = command.ExecuteScalar();
                return result != DBNull.Value ? (dynamic)result : null;
            }
        }

        public static T? ExecuteScalar<T>(this SqlConnection connection, string sql)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();
            using (var command = new SqlCommand(sql, connection))
            {
                var result = command.ExecuteScalar();
                return result != DBNull.Value ? (T)result : default;
            }
        }

        public static async Task<dynamic?> ExecuteScalarAsync(this SqlConnection connection, string sql)
        {
            if (connection.State == ConnectionState.Closed) await connection.OpenAsync();
            using (var command = new SqlCommand(sql, connection))
            {
                var result = await command.ExecuteScalarAsync();
                return result != DBNull.Value ? (dynamic)result : null;
            }
        }

        public static async Task<T?> ExecuteScalarAsync<T>(this SqlConnection connection, string sql)
        {
            if (connection.State == ConnectionState.Closed) await connection.OpenAsync();
            using (var command = new SqlCommand(sql, connection))
            {
                var result = await command.ExecuteScalarAsync();
                return result != DBNull.Value ? (T?)result : default;
            }
        }
    }
}
