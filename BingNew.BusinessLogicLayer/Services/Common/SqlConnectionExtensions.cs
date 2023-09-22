using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BingNew.BusinessLogicLayer.Services.Common
{
    public static class SqlConnectionExtensions
    {
        public static T QuerySingle<T>(this SqlConnection connection, string sql, int? commandTimeout = null, SqlParameter[]? sqlParameters = null, IDbTransaction? transaction = null)
            where T : class, new()
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.Connection = connection;
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                command.CommandTimeout = commandTimeout ?? 30;

                if (sqlParameters != null && sqlParameters.Length > 0)
                {
                    command.Parameters.AddRange(sqlParameters);
                }
                using (var reader = command.ExecuteReader())
                {
                    return reader.Read() ? ConvertToObject<T>(reader) : throw new InvalidOperationException("Invalid return data: zero or more than one element");
                }
            }
        }
        public static T QuerySingleOrDefault<T>(this SqlConnection connection, string sql, int? commandTimeout = null, SqlParameter[] sqlParameters = null, IDbTransaction transaction = null)
            where T : class, new()
        {
            using (var command = connection.CreateCommand())
            {
                command.Connection = connection;
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                command.CommandTimeout = commandTimeout ?? 30;

                T result = null;
                bool hasResult = false; 
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (hasResult)
                        {
                            throw new InvalidOperationException("Multiple rows found");
                        }
                        result = ConvertToObject<T>(reader);
                        hasResult = true;
                    }
                }
                if (!hasResult)
                {
                    return null;
                }
                return result;
            }
        }
        public static T QueryFirst<T>(this SqlConnection connection, string sql, int? commandTimeout = null, SqlParameter[] sqlParameters = null, IDbTransaction transaction = null)
            where T : class, new()
        {
            using (var command = connection.CreateCommand())
            {
                command.Connection = connection;
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                command.CommandTimeout = commandTimeout ?? 30;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return ConvertToObject<T>(reader);
                    }
                }
                throw new InvalidOperationException("Invalid return data: zero element");
            }
        }

        public static dynamic QuerySingle(this SqlConnection connectionn, string sql)
        {
            using (var command = new SqlCommand(sql, connectionn))
            {
                using (var reader = command.ExecuteReader())
                {
                    int rowCount = 0;
                    if (reader.HasRows && reader.Read())
                    {
                        var typeName = ExtractTypeNameFromSql(sql);
                        var resultType = FindTypeByName(typeName);
                        if (resultType != null)
                        {
                            var obj = Activator.CreateInstance(resultType);
                            do
                            {
                                rowCount++;
                                if (rowCount > 1)
                                {
                                    throw new InvalidOperationException("Invalid return data: more than one element");
                                }
                                foreach (var propertyInfo in resultType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                                {
                                    var columnName = propertyInfo.Name;
                                    if (reader.HasColumn(columnName) && !reader.IsDBNull(reader.GetOrdinal(columnName)))
                                    {
                                        var propertyValue = reader[columnName];
                                        propertyInfo.SetValue(obj, propertyValue);
                                    }
                                }
                            } while (reader.Read());

                            return obj ?? throw new InvalidOperationException("Invalid return data: zero");
                        }
                    }
                    throw new InvalidOperationException("Invalid return data: zero");
                }
            }
        }

        public static dynamic QuerySingleOrDefault(this SqlConnection connectionn, string sql)
        {
            using (var command = new SqlCommand(sql, connectionn))
            {
                using (var reader = command.ExecuteReader())
                {
                    dynamic result = null;
                    int rowCount = 0;
                    while (reader.Read())
                    {
                        rowCount++;
                        if (rowCount > 1)
                        {
                            throw new InvalidOperationException("Invalid return data: more than one element");
                        }
                        result = new System.Dynamic.ExpandoObject();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            ((IDictionary<string, object>)result)[columnName] = reader[i];
                        }
                    }

                    if (rowCount == 0)
                    {
                        return null;
                    }
                    return result;
                }
            }
        }

        public static string GetTypeNameFromSql(string sql)
        {
            var fromIndex = sql.IndexOf("FROM");
            if (fromIndex >= 0)
            {
                var afterFrom = sql.Substring(fromIndex + 5).Trim();
                var spaceIndex = afterFrom.IndexOf(' ');
                if (spaceIndex >= 0)
                {
                    return afterFrom.Substring(0, spaceIndex);
                }
                return afterFrom;
            }

            throw new InvalidOperationException("Invalid SQL query");
        }

        public static object QuerySingle2(this SqlConnection connection, string sql)
        {
            using (var command = new SqlCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows && reader.Read())
                    {
                        var typeName = ExtractTypeNameFromSql(sql);
                        if (!string.IsNullOrEmpty(typeName))
                        {
                            var resultType = FindTypeByName(typeName);
                            if (resultType != null)
                            {
                                var obj = Activator.CreateInstance(resultType);

                                foreach (var propertyInfo in resultType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                                {
                                    var columnName = propertyInfo.Name;
                                    if (reader.HasColumn(columnName) && !reader.IsDBNull(reader.GetOrdinal(columnName)))
                                    {
                                        var propertyValue = reader[columnName];
                                        propertyInfo.SetValue(obj, propertyValue);
                                    }
                                }

                                return obj;
                            }
                        }
                        return null;
                    }
                }
            }
            return null;
        }

        private static Type? FindTypeByName(string? typeName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var type = assembly.GetType(typeName);
                type = assembly.GetTypes().FirstOrDefault(t => t.Name == typeName);
                if (type != null)
                {
                    return type;
                }
            }
            return null;
        }

        public static string? ExtractTypeNameFromSql(string? sql)
        {
            var tableNameStartIndex = sql.IndexOf("FROM ", StringComparison.OrdinalIgnoreCase);
            if (tableNameStartIndex >= 0)
            {
                tableNameStartIndex += 5; // FROM 
                var tableNameEndIndex = sql.IndexOf(" ", tableNameStartIndex, StringComparison.OrdinalIgnoreCase);
                if (tableNameEndIndex > tableNameStartIndex)
                {
                    return sql.Substring(tableNameStartIndex, tableNameEndIndex - tableNameStartIndex);
                }
            }
            return null;
        }

        public static T ConvertToObject<T>(IDataReader reader) where T : new()
        {
            var obj = new T();
            var type = typeof(T);
            var properties = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var columnName = property.Name; // get field name
                if (reader.HasColumn(columnName) && !reader.IsDBNull(reader.GetOrdinal(columnName)))
                {
                    var propertyValue = reader[columnName];
                    property.SetValue(obj, propertyValue);
                }
            }

            return obj;
        }

        public static bool HasColumn(this IDataReader reader, string columnName)
        {
            for (var i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
