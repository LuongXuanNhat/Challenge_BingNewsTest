using System.Data;
using System.Reflection;

namespace BingNew.ORM.Query
{
    public static class SqlExtensionCommon
    {
        public static string ExtractTypeNameFromSql(string sql)
        {
            var tableNameStartIndex = sql.IndexOf("FROM ", StringComparison.OrdinalIgnoreCase);
            return tableNameStartIndex >= 0
                ? sql[(tableNameStartIndex + 5)..]
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault()!
                : throw new Exception("Table name not found in SQL");
        }


        public static Type FindTypeByName(string typeName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type? type = null;
            foreach (var assembly in assemblies)
            {
                type ??= assembly.GetTypes().ToList().Find(x => x.Name.Equals(typeName));
            }
            return type ?? throw new NullReferenceException("Not found!");
        }
        public static T ConvertToObject<T>(IDataReader reader) where T : new()
        {
            var obj = new T();
            var type = typeof(T);
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var columnName = property.Name;
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
