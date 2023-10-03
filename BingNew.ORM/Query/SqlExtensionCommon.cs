using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace BingNew.ORM.Query
{
    public static class SqlExtensionCommon
    {
        public static void OpenOrClose(this SqlConnection sqlConnection, ConnectionState state)
        {
            switch (state)
            {
                case ConnectionState.Closed:
                    sqlConnection.Open();
                    break;

            }
        }
        public static string ExtractTypeNameFromSql(string sql)
        {
            var tableNameStartIndex = sql.IndexOf("FROM ", StringComparison.OrdinalIgnoreCase);
            return tableNameStartIndex >= 0
                ? sql[(tableNameStartIndex + 5)..]
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault()!
                : string.Empty;
        }
        public static Type FindTypeByName(string typeName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type? type = null;
            foreach (var assembly in assemblies)
            {
                type ??= assembly.GetTypes().ToList().Find(x => x.Name.Equals(typeName));
            }
            return type ?? throw new InvalidOperationException("Type not found!");
        }
        public static T ConvertToObject<T>(IDataReader reader) where T : new()
        {
            var obj = new T();
            var type = typeof(T);
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var columnName = property.Name;
                var propertyValue = reader[columnName];
                property.SetValue(obj, propertyValue);
            }

            return obj;
        }
        public static bool HasColumn(this IDataReader reader, string columnName)
        {
            int? check = null;
            for (var i = 0; i < reader.FieldCount; i++)
            {
                check ??= (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase)) ? 1 : null;
            }
            return check == 1;
        }
    }
}
