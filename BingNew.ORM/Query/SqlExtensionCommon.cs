using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BingNew.ORM.Query
{
    public static class SqlExtensionCommon
    {
        public static string? ExtractTypeNameFromSql(string? sql)
        {
            if (sql != null)
            {
                var tableNameStartIndex = sql.IndexOf("FROM ", StringComparison.OrdinalIgnoreCase);
                if (tableNameStartIndex >= 0)
                {
                    tableNameStartIndex += 5;
                    var tableNameSubstring = sql.Substring(tableNameStartIndex);
                    var tableNameEndIndex = tableNameSubstring.IndexOf(' ');
                    if (tableNameEndIndex == -1)
                    {
                        return tableNameSubstring;
                    }
                    else
                    {
                        return tableNameSubstring.Substring(0, tableNameEndIndex);
                    }
                }
            }
            return null;
        }
        public static Type? FindTypeByName(string? typeName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            if (typeName == null)
            {
                return null;
            }
            foreach (var assembly in assemblies)
            {
                var type = assembly.GetTypes().FirstOrDefault(x=>x.Name.Equals(typeName));
                if (type != null)
                {
                    return type;
                }
            }
            return null;
        }
        // ReSharper disable S3011
        public static T ConvertToObject<T>(IDataReader reader) where T : new()
        {
            var obj = new T();
            var type = typeof(T);
            var properties = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

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
        // ReSharper restore S3011
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
