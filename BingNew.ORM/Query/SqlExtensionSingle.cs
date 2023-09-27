using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace BingNew.ORM.Query
{
#pragma warning disable S3011
    public static class SqlExtensionSingle
    {
        public static T QuerySingle<T>(this SqlConnection connection, string sql, int? commandTimeout = null, SqlParameter[]? sqlParameters = null, IDbTransaction? transaction = null)
            where T : class, new()
        {
            if (connection.State == ConnectionState.Closed) connection.Open();
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
        public static dynamic QuerySingle(this SqlConnection connection, string sql)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            using (var command = new SqlCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    int rowCount = 0;
                    var obj = default(dynamic);
                    var resultType = GetResultType(sql);

                    while (reader.Read())
                    {
                        rowCount++;
                        if (rowCount > 1)
                        {
                            throw new InvalidOperationException("Invalid return data: more than one element");
                        }
                        if (resultType != null)
                        {
                            obj = CreateInstance(resultType, reader);
                        }
                    }

                    return obj ?? throw new InvalidOperationException("Invalid return data: zero");
                }
            }
        }

        private static Type? GetResultType(string sql)
        {
            var typeName = SqlExtensionCommon.ExtractTypeNameFromSql(sql);
            return SqlExtensionCommon.FindTypeByName(typeName);
        }

        private static dynamic? CreateInstance(Type? resultType, SqlDataReader reader)
        {
            if (resultType == null) return null;
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

        public static dynamic? QuerySingleOrDefault(this SqlConnection connection, string sql)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();
            var typeName = SqlExtensionCommon.ExtractTypeNameFromSql(sql);
            var resultType = SqlExtensionCommon.FindTypeByName(typeName);
            if (resultType == null) return null;

            using (var command = new SqlCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    var obj = Activator.CreateInstance(resultType);
                    if (reader.Read())
                    {
                        foreach (var propertyInfo in resultType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                        {
                            if (reader.HasColumn(propertyInfo.Name) && !reader.IsDBNull(reader.GetOrdinal(propertyInfo.Name)))
                            {
                                var propertyValue = reader[propertyInfo.Name];
                                propertyInfo.SetValue(obj, propertyValue);
                            }
                        }
                        if (reader.Read())
                        {
                            throw new InvalidOperationException("Invalid return data: more than one element");
                        }
                        return obj;
                    }
                    else
                    {
                        return null; 
                    }
                }
            }
        }

        public static T? QuerySingleOrDefault<T>(this SqlConnection connection, string sql, int? commandTimeout = null, SqlParameter[]? sqlParameters = null, IDbTransaction? transaction = null)
            where T : class, new()
        {
            if (connection.State == ConnectionState.Closed) connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.Connection = connection;
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                command.CommandTimeout = commandTimeout ?? 30;

                T? result = null;
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
        public static dynamic QueryFirst(this SqlConnection connection, string sql)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();
            return QueryExcute(connection, sql) ?? throw new InvalidOperationException("Invalid return data: zero");
        }
        public static T QueryFirst<T>(this SqlConnection connection, string sql, int? commandTimeout = null, SqlParameter[]? sqlParameters = null, IDbTransaction? transaction = null)
            where T : class, new()
        {
            if (connection.State == ConnectionState.Closed) connection.Open();
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
            if (connection.State == ConnectionState.Closed) connection.Open();
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
        

        public static dynamic? QueryFirstOrDefault(this SqlConnection connection, string sql)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();
            return QueryExcute(connection, sql);
            
        }

        private static dynamic? QueryExcute(SqlConnection connection, string sql)
        {
            using (var command = new SqlCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    var typeName = SqlExtensionCommon.ExtractTypeNameFromSql(sql);
                    var resultType = SqlExtensionCommon.FindTypeByName(typeName);
                    if (resultType != null && reader.Read())
                    {
                        var obj = Activator.CreateInstance(resultType);
                        var properties = resultType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                        foreach (var propertyInfo in properties)
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
                    return null;
                }
            }
        }
    }
}
