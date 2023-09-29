using System.Data.SqlClient;
using System.Data;

namespace BingNew.ORM.Query
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
                using (var reader = command.ExecuteReader())
                {
                    return reader.Read() ? SqlExtensionCommon.ConvertToObject<T>(reader) : throw new InvalidOperationException("Invalid return data: zero or more than one element");
                }
            }
        }
        public static dynamic QuerySingle(this SqlConnection connection, string sql)
        {
            connection.Open();
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            int rowCount = 0;
            var obj = default(dynamic);
            var resultType = GetResultType(sql);

            while (reader.Read())
            {
                rowCount++;
                obj = (rowCount > 1) ? throw new InvalidOperationException("Invalid return data: more than one element")
                    : MapObject(resultType, reader);
            }
            return obj ?? throw new InvalidOperationException("Invalid return data: more than one element");
        }

        private static Type GetResultType(string sql)
        {
            var typeName = SqlExtensionCommon.ExtractTypeNameFromSql(sql);
            return SqlExtensionCommon.FindTypeByName(typeName) ?? throw new NullReferenceException("Type not found");
        }

        private static dynamic MapObject(Type resultType, SqlDataReader reader)
        {
            var obj = Activator.CreateInstance(resultType);
            foreach (var propertyInfo in resultType.GetProperties())
            {
                var columnName = propertyInfo.Name;
                var propertyValue = reader.HasColumn(columnName) && !reader.IsDBNull(reader.GetOrdinal(columnName))
                    ? reader[columnName]
                    : null;

                propertyInfo.SetValue(obj, propertyValue);
            }
            return obj ?? throw new NullReferenceException("Instance of object is null");
        }

        public static dynamic? QuerySingleOrDefault(this SqlConnection connection, string sql)
        {
            connection.Open();
            var typeName = SqlExtensionCommon.ExtractTypeNameFromSql(sql);
            var resultType = SqlExtensionCommon.FindTypeByName(typeName);
            int rowCount = 0;
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            dynamic? obj = null;

            while (reader.Read())
            {
                rowCount++;
                obj = (rowCount > 1) ? throw new InvalidOperationException("Invalid return data: more than one element")
                    : MapObject(resultType, reader);
            }
            return obj ?? throw new NullReferenceException("Instance of object is null");
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
                        var properties = resultType.GetProperties();
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
