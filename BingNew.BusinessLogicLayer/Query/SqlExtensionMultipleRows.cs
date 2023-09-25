using Dasync.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace BingNew.BusinessLogicLayer.Query
{
    public static class SqlExtensionMultipleRows
    {
        public static IEnumerable<dynamic> Query(this SqlConnection connection, string sql)
        {
            if (connection.State == ConnectionState.Closed)  connection.Open();
            using (var command = new SqlCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    var typeName = SqlExtensionCommon.ExtractTypeNameFromSql(sql);
                    var resultType = SqlExtensionCommon.FindTypeByName(typeName);
                    if (resultType != null)
                    {
                        while (reader.Read())
                        {
                            var obj = Activator.CreateInstance(resultType);
                            for (var i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var propertyInfo = resultType.GetField(columnName, BindingFlags.NonPublic | BindingFlags.Instance);
                                if (propertyInfo != null && !reader.IsDBNull(i))
                                {
                                    var value = reader.GetValue(i);
                                    propertyInfo.SetValue(obj, value);
                                }
                            }
                            yield return obj;
                        }
                    }
                }
            }
        }

        public static async IAsyncEnumerable<dynamic> QueryAsync(this SqlConnection connection, string sql)
        {
            if (connection.State == ConnectionState.Closed) await connection.OpenAsync();
            using (var command = new SqlCommand(sql, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var typeName = SqlExtensionCommon.ExtractTypeNameFromSql(sql);
                    var resultType = SqlExtensionCommon.FindTypeByName(typeName);
                    if (resultType != null)
                    {
                        while (await reader.ReadAsync())
                        {
                            var obj = Activator.CreateInstance(resultType);
                            for (var i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var propertyInfo = resultType.GetField(columnName, BindingFlags.NonPublic | BindingFlags.Instance);
                                if (propertyInfo != null && !reader.IsDBNull(i))
                                {
                                    var value = reader.GetValue(i);
                                    propertyInfo.SetValue(obj, value);
                                }
                            }
                            yield return obj;
                        }
                    }
                }
            }
        }




        public static IEnumerable<T> Query<T>(this SqlConnection connection, string sql)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();
            using (var command = new SqlCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = Activator.CreateInstance<T>();
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            var propertyInfo = typeof(T).GetField(columnName, BindingFlags.NonPublic | BindingFlags.Instance);
                            if (propertyInfo != null && !reader.IsDBNull(i))
                            {
                                var value = reader.GetValue(i);
                                propertyInfo.SetValue(obj, value);
                            }
                        }
                        yield return obj;
                    }
                }
            }
        }

        public static async IAsyncEnumerable<T> QueryAsync<T>(this SqlConnection connection, string sql)
        {
            if (connection.State == ConnectionState.Closed) await connection.OpenAsync();
            using (var command = new SqlCommand(sql, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var obj = Activator.CreateInstance<T>();
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            var propertyInfo = typeof(T).GetField(columnName, BindingFlags.NonPublic | BindingFlags.Instance);
                            if (propertyInfo != null && !reader.IsDBNull(i))
                            {
                                var value = reader.GetValue(i);
                                propertyInfo.SetValue(obj, value);
                            }
                        }
                        yield return obj;
                    }
                }
            }
        }
    }
}
