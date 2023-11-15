using System.Data.SqlClient;

namespace BingNew.ORM.Query
{
    public static class SqlExtensionMultipleRows
    {
        public static IEnumerable<dynamic?> Query(this SqlConnection sqlConnection, string sql)
        {
            sqlConnection.SqlConnectionManager(sqlConnection.State);

            var typeName = SqlExtensionCommon.ExtractTypeNameFromSql(sql);
            var resultType = SqlExtensionCommon.FindTypeByName(typeName);

            return ReadValue(sqlConnection, sql, resultType);
        }
        public static IEnumerable<T> Query<T>(this SqlConnection connection, string sql)
        {
            connection.SqlConnectionManager(connection.State);
            foreach (var item in ReadValue(connection, sql, typeof(T)))
            {
                yield return item;
            }
        }

        private static IEnumerable<dynamic> ReadValue(SqlConnection sqlConnection, string sql, Type resultType)
        {
            using var command = new SqlCommand(sql, sqlConnection);
            using var reader = command.ExecuteReader();
            while (reader.Read() && resultType != null)
            {
                var obj = Activator.CreateInstance(resultType);
                MapDataToObject(reader, resultType, obj);
                yield return obj ?? throw new InvalidOperationException("object is null");
            }
        }

        public static async IAsyncEnumerable<dynamic?> QueryAsync(this SqlConnection connection, string sql)
        {
            connection.SqlConnectionManager(connection.State);
            var typeName = SqlExtensionCommon.ExtractTypeNameFromSql(sql);
            var resultType = SqlExtensionCommon.FindTypeByName(typeName);
            await foreach (var item in ReadValueAsync(connection, sql, resultType))
            {
                yield return item;
            }
        }

        public static async IAsyncEnumerable<T> QueryAsync<T>(this SqlConnection connection, string sql, string? param = null)
        {
            connection.SqlConnectionManager(connection.State);
            await foreach (var item in ReadValueAsync(connection, sql, typeof(T), param))
            {
                yield return item;
            }
        }

        private static async IAsyncEnumerable<dynamic> ReadValueAsync(SqlConnection connection, string sql, Type? resultType, string? param = null)
        {
            using var command = new SqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();
            _ = (param != null) ? command.Parameters.AddWithValue("@keyWord", param) : default;
            while (await reader.ReadAsync() && resultType != null)
            {
                var obj = Activator.CreateInstance(resultType) ?? throw new InvalidOperationException("item is null");
                MapDataToObject(reader, resultType, obj);
                yield return obj;
            }
        }
        private static void MapDataToObject(SqlDataReader reader, Type resultType, object? obj)
        {
            for (var i = 0; i < reader.FieldCount; i++)
            {
                var columnName = reader.GetName(i);
                var propertyInfo = resultType.GetProperty(columnName);
                var value = reader[i];

                propertyInfo?.SetValue(obj, value is DBNull ? null : value);
            }
        }

    }
}
