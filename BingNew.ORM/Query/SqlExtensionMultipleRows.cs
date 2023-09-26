﻿using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace BingNew.ORM.Query
{
    public static class SqlExtensionMultipleRows
    {
        public static IEnumerable<dynamic?>? Query(this SqlConnection sqlConnection, string sql)
        {
            if (sqlConnection.State == ConnectionState.Closed) sqlConnection.Open();
            var typeName = SqlExtensionCommon.ExtractTypeNameFromSql(sql);
            var resultType = SqlExtensionCommon.FindTypeByName(typeName);
            if (resultType != null)
            {
                return ReadValue(sql, sqlConnection, resultType);
            } return default(dynamic);
        }
        public static IEnumerable<T> Query<T>(this SqlConnection connection, string sql)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();
            foreach (var item in ReadValue(sql, connection, typeof(T)))
            {
                yield return item;
            }
            //using (var command = new SqlCommand(sql, connection))
            //{
            //    using (var reader = command.ExecuteReader())
            //    {
            //        while (reader.Read())
            //        {
            //            var obj = Activator.CreateInstance<T>();
            //            for (var i = 0; i < reader.FieldCount; i++)
            //            {
            //                var columnName = reader.GetName(i);
            //                var propertyInfo = typeof(T).GetField(columnName, BindingFlags.NonPublic | BindingFlags.Instance);
            //                if (propertyInfo != null && !reader.IsDBNull(i))
            //                {
            //                    var value = reader.GetValue(i);
            //                    propertyInfo.SetValue(obj, value);
            //                }
            //            }
            //            yield return obj;
            //        }
            //    }
            //}
        }

        private static IEnumerable<dynamic?> ReadValue(string sql, SqlConnection sqlConnection, Type resultType)
        {
            using (var command = new SqlCommand(sql, sqlConnection))
            {
                using (var reader = command.ExecuteReader())
                {
                    var obj = Activator.CreateInstance(resultType);
                    while (reader.Read())
                    {
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

        public static async IAsyncEnumerable<dynamic> QueryAsync(this SqlConnection connection, string sql)
        {
            if (connection.State == ConnectionState.Closed) await connection.OpenAsync();
            var typeName = SqlExtensionCommon.ExtractTypeNameFromSql(sql);
            var resultType = SqlExtensionCommon.FindTypeByName(typeName);
            await foreach (var item in ReadValueAsync(connection, sql, resultType))
            {
                yield return item;
            }
        }

        public static async IAsyncEnumerable<T> QueryAsync<T>(this SqlConnection connection, string sql)
        {
            if (connection.State == ConnectionState.Closed) await connection.OpenAsync();
            await foreach (var item in ReadValueAsync(connection, sql, typeof(T)))
            {
                yield return item;
            }
        }

        private static async IAsyncEnumerable<dynamic?> ReadValueAsync(SqlConnection connection, string sql, Type? resultType)
        {
            using (var command = new SqlCommand(sql, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
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
    }
}
