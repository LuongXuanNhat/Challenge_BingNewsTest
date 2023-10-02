using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace BingNew.ORM.Query
{
    public static class SqlExtensionSingle
    {
        public static T QuerySingle<T>(this SqlConnection connection, string sql, int? commandTimeout = null, SqlParameter[]? sqlParameters = null, IDbTransaction? transaction = null)
            where T : class, new()
        {
            connection.OpenOrClose(connection.State);
            using var command = connection.CreateCommand();
            command.Connection = connection;
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            command.CommandTimeout = commandTimeout ?? 30;
            using var reader = command.ExecuteReader();
            return reader.Read() ? SqlExtensionCommon.ConvertToObject<T>(reader) : throw new InvalidOperationException("Invalid return data: zero or more than one element");
        }
        public static dynamic QuerySingle(this SqlConnection connection, string sql)
        {
            connection.OpenOrClose(connection.State);
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            var obj = default(dynamic);
            var resultType = GetResultType(sql);

            obj = (reader.Read() && resultType != null) ? MapObject(resultType, reader) : null;

            return reader.Read() ? throw new InvalidOperationException("Invalid return data: more than one element") 
                : obj ?? throw new NullReferenceException("Invalid return data: zero element");
        }

        private static Type? GetResultType(string sql)
        {
            var typeName = SqlExtensionCommon.ExtractTypeNameFromSql(sql);
            return SqlExtensionCommon.FindTypeByName(typeName);
        }

        private static dynamic MapObject(Type resultType, SqlDataReader reader)
        {
            var obj = Activator.CreateInstance(resultType);
            foreach (var propertyInfo in resultType.GetProperties())
            {
                var columnName = propertyInfo.Name;
                var propertyValue = reader.HasColumn(columnName) 
                    ? reader[columnName]
                    : null;
                propertyInfo.SetValue(obj, propertyValue);
            }
            return obj ?? throw new InvalidOperationException("Instance of object is null");
        }
        public static dynamic? QuerySingleOrDefault(this SqlConnection connection, string sql)
        {
            connection.OpenOrClose(connection.State);
            var typeName = SqlExtensionCommon.ExtractTypeNameFromSql(sql);
            var resultType = SqlExtensionCommon.FindTypeByName(typeName) ;
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            dynamic? obj = null;

            obj = (reader.Read() && resultType != null) ? MapObject(resultType, reader) : null;

            return reader.Read() ? throw new InvalidOperationException("Invalid return data: more than one element") : obj;
        }
        public static T? QuerySingleOrDefault<T>(this SqlConnection connection, string sql, int? commandTimeout = null, SqlParameter[]? sqlParameters = null, IDbTransaction? transaction = null)
            where T : class, new()
        {
            connection.OpenOrClose(connection.State);
            using var command = connection.CreateCommand();
            command.CommandText = sql;

            T? result = null;
            using var reader = command.ExecuteReader();
            result = (reader.Read()) ? SqlExtensionCommon.ConvertToObject<T>(reader) : null;

            return (reader.Read()) ? throw new InvalidOperationException("Multiple rows found") : result;


        }
        public static dynamic QueryFirst(this SqlConnection connection, string sql)
        {
            connection.OpenOrClose(connection.State);
            return QueryExcute(connection, sql) ?? throw new InvalidOperationException("Invalid return data: zero");
        }
        public static T QueryFirst<T>(this SqlConnection connection, string sql, int? commandTimeout = null, SqlParameter[]? sqlParameters = null, IDbTransaction? transaction = null)
            where T : class, new()
        {
            connection.OpenOrClose(connection.State);
            using var command = connection.CreateCommand();
            command.Connection = connection;
            command.CommandText = sql;
            command.CommandType = CommandType.Text;

            using var reader = command.ExecuteReader();
            return reader.Read() ? SqlExtensionCommon.ConvertToObject<T>(reader) 
                : throw new InvalidOperationException("Invalid return data: zero or more than one element");

        }
        public static T? QueryFirstOrDefault<T>(this SqlConnection connection, string sql, int? commandTimeout = null, SqlParameter[]? sqlParameters = null, IDbTransaction? transaction = null)
            where T : class, new()
        {
            connection.OpenOrClose(connection.State);
            using var command = connection.CreateCommand();
            command.Connection = connection;
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            command.CommandTimeout = commandTimeout ?? 30;

            using var reader = command.ExecuteReader();
            return reader.Read() ? SqlExtensionCommon.ConvertToObject<T>(reader) : null;
        }
        public static dynamic? QueryFirstOrDefault(this SqlConnection connection, string sql)
        {
            connection.OpenOrClose(connection.State);
            return QueryExcute(connection, sql);
        }
        private static dynamic? QueryExcute(SqlConnection connection, string sql)
        {
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            var typeName = SqlExtensionCommon.ExtractTypeNameFromSql(sql);
            var resultType = SqlExtensionCommon.FindTypeByName(typeName);
            return resultType != null
            ? reader.Cast<IDataRecord>()
                .Select(dataRecord =>
                {
                    var obj = Activator.CreateInstance(resultType);
                    var properties = resultType.GetProperties();
                    foreach (var propertyInfo in properties)
                    {
                        var columnName = propertyInfo.Name;
                        var propertyValue = reader.HasColumn(columnName)
                            ? dataRecord[columnName]
                            : null;
                        propertyInfo.SetValue(obj, propertyValue);
                    }
                    return obj;
                })
                .FirstOrDefault()
            : null;
        }
    }
}
