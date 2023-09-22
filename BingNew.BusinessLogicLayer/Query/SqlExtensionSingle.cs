using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BingNew.BusinessLogicLayer.Query
{
    public static class SqlExtensionSingle
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
                    return reader.Read() ? SqlExtensionCommon.ConvertToObject<T>(reader) : throw new InvalidOperationException("Invalid return data: zero or more than one element");
                }
            }
        }
        public static T? QuerySingleOrDefault<T>(this SqlConnection connection, string sql, int? commandTimeout = null, SqlParameter[]? sqlParameters = null, IDbTransaction? transaction = null)
            where T : class, new()
        {
            connection.Open();
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
                        result = SqlExtensionCommon.ConvertToObject<T>(reader);
                        hasResult = true;
                    }
                }
                return result ?? null;
            }
        }
        public static T QueryFirst<T>(this SqlConnection connection, string sql, int? commandTimeout = null, SqlParameter[]? sqlParameters = null, IDbTransaction? transaction = null)
            where T : class, new()
        {
            connection.Open();
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
                        return SqlExtensionCommon.ConvertToObject<T>(reader);
                    }
                }
                throw new InvalidOperationException("Invalid return data: zero or more than one element");
            }
        }
        public static T? QueryFirstOrDefault<T>(this SqlConnection connection, string sql, int? commandTimeout = null, SqlParameter[]? sqlParameters = null, IDbTransaction? transaction = null)
            where T : class, new()
        {
            connection.Open();
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
                        return SqlExtensionCommon.ConvertToObject<T>(reader);
                    }
                }
                return null;
            }
        }

        public static dynamic QuerySingle(this SqlConnection connectionn, string sql)
        {
            connectionn.Open();
            using (var command = new SqlCommand(sql, connectionn))
            {
                using (var reader = command.ExecuteReader())
                {
                    int rowCount = 0;
                    var obj = default(dynamic);
                    var typeName = SqlExtensionCommon.ExtractTypeNameFromSql(sql);
                    var resultType = SqlExtensionCommon.FindTypeByName(typeName);
                    while (reader.Read())
                    {
                        rowCount++;
                        if (rowCount > 1)
                        {
                            throw new InvalidOperationException("Invalid return data: more than one element");
                        }
                        if (resultType != null)
                        {
                            obj = Activator.CreateInstance(resultType);
                            foreach (var propertyInfo in resultType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                            {
                                var columnName = propertyInfo.Name;
                                if (reader.HasColumn(columnName) && !reader.IsDBNull(reader.GetOrdinal(columnName)))
                                {
                                    var propertyValue = reader[columnName];
                                    propertyInfo.SetValue(obj, propertyValue);
                                }
                            }
                        }
                    }
                    return obj ?? throw new InvalidOperationException("Invalid return data: zero");
                }
            }
        }

        public static dynamic? QuerySingleOrDefault(this SqlConnection connectionn, string sql)
        {
            connectionn.Open();
            using (var command = new SqlCommand(sql, connectionn))
            {
                using (var reader = command.ExecuteReader())
                {
                    int rowCount = 0;
                    if (reader.HasRows && reader.Read())
                    {
                        var typeName = SqlExtensionCommon.ExtractTypeNameFromSql(sql);
                        var resultType = SqlExtensionCommon.FindTypeByName(typeName);
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
                                    if (reader.HasColumn(propertyInfo.Name) && !reader.IsDBNull(reader.GetOrdinal(propertyInfo.Name)))
                                    {
                                        var propertyValue = reader[propertyInfo.Name];
                                        propertyInfo.SetValue(obj, propertyValue);
                                    }
                                }
                            } while (reader.Read());
                            return obj ?? null;
                        }
                    }
                    return null;
                }
            }
        }

        public static dynamic QueryFirst(this SqlConnection connectionn, string sql)
        {
            connectionn.Open();
            using (var command = new SqlCommand(sql, connectionn))
            {
                using (var reader = command.ExecuteReader())
                {
                    var obj = default(dynamic);
                    var typeName = SqlExtensionCommon.ExtractTypeNameFromSql(sql);
                    var resultType = SqlExtensionCommon.FindTypeByName(typeName);
                    if (resultType != null && reader.Read())
                    {
                        var properties = resultType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                        obj = Activator.CreateInstance(resultType);
                        foreach (var propertyInfo in properties)
                        {
                            var columnName = propertyInfo.Name;
                            if (reader.HasColumn(columnName) && !reader.IsDBNull(reader.GetOrdinal(columnName)))
                            {
                                var propertyValue = reader[columnName];
                                propertyInfo.SetValue(obj, propertyValue);
                            }
                        }
                    }
                    return obj ?? throw new InvalidOperationException("Invalid return data: zero");
                }
            }
        }

        public static dynamic? QueryFirstOrDefault(this SqlConnection connectionn, string sql)
        {
            connectionn.Open();
            using (var command = new SqlCommand(sql, connectionn))
            {
                using (var reader = command.ExecuteReader())
                {
                    var obj = default(dynamic);
                    var typeName = SqlExtensionCommon.ExtractTypeNameFromSql(sql);
                    var resultType = SqlExtensionCommon.FindTypeByName(typeName);
                    if (resultType != null && reader.Read())
                    {
                        var properties = resultType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                        obj = Activator.CreateInstance(resultType);
                        foreach (var propertyInfo in properties)
                        {
                            var columnName = propertyInfo.Name;
                            if (reader.HasColumn(columnName) && !reader.IsDBNull(reader.GetOrdinal(columnName)))
                            {
                                var propertyValue = reader[columnName];
                                propertyInfo.SetValue(obj, propertyValue);
                            }
                        }
                    }
                    return obj ?? null;
                }
            }
        }

    }
}
