using BingNew.ORM.Query;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;

namespace BingNew.ORM.NonQuery
{
    public static class SqlExtensionNonQuery
    {
        public static bool Insert<T>(this SqlConnection connection,T entity)
        {
                var sql = GenerateInsertQuery(entity);
                return QueryExcute<T>(connection, sql, entity);
        }

        private static bool QueryExcute<T>(SqlConnection connection, string sql, T entity)
        {
            connection.OpenOrClose(connection.State);
            using var command = new SqlCommand(sql, connection);
            foreach (var property in typeof(T).GetProperties())
            {
                var paramName = "@" + property.Name;
                var value = property.GetValue(entity);
                command.Parameters.AddWithValue(paramName, value ?? DBNull.Value);
            }

            command.ExecuteNonQuery();
            return true;
        }

        public static string GenerateInsertQuery<T>(T entity)
        {
            string tableName = typeof(T).Name;
            PropertyInfo[] properties = typeof(T).GetProperties();
            string columns = string.Join(", ", properties.Select(p => p.Name));
            string values = string.Join(", ", properties.Select(p => "@" + p.Name));
            string query = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

            return query;
        }

        /// <summary>
        /// Cập nhập 1 hàng trong bảng dữ liệu
        /// </summary>
        /// <param name="entity"> Đối tượng được cập nhập</param>
        /// <returns> Trả về true nếu cập nhập thành công và ngược lại </returns>
        public static bool Update<T>(this SqlConnection connection, T entity)
        {
            try
            {
                connection.OpenOrClose(connection.State);

                var sql = GenerateUpdateQuery(entity);
                return QueryExcute<T>(connection, sql, entity);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("XUẤT HIỆN LỖI KHÔNG MONG MUỐN: " + ex.Message.ToString());
                return false;
            }
        }

        public static string GenerateUpdateQuery<T>(T entity)
        {
            string tableName = typeof(T).Name;
            PropertyInfo[] properties = typeof(T).GetProperties();
            string setClause = string.Join(", ", properties.Select(p => p.Name + " = @" + p.Name));
            string query = $"UPDATE {tableName} SET {setClause} WHERE Id = @Id";

            return query;
        }

        public static bool Delete<T>(this SqlConnection connection, Guid entityId) where T : class
        {
            try
            {
                connection.OpenOrClose(connection.State);

                string query = GenerateDeleteQuery<T>();

                using var command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@Id", entityId));
                command.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("XUẤT HIỆN LỖI KHÔNG MONG MUỐN: " + ex.Message.ToString());
                return false;
            }

        }

        private static string GenerateDeleteQuery<T>() where T : class
        {
            return "DELETE " + typeof(T).Name + " WHERE Id = @Id";
        }

        public static T? GetById<T>(this SqlConnection connection, Guid entityId) where T : new()
        {
            try
            {
                connection.OpenOrClose(connection.State);
                var sql = GenerateGetObjectQuery<T>();

                using var command = new SqlCommand(sql, connection);
                command.Parameters.Add(new SqlParameter("@Id", entityId));
                using var reader = command.ExecuteReader();

                return reader.Read() ? SqlExtensionCommon.ConvertToObject<T>(reader) : default;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("XUẤT HIỆN LỖI KHÔNG MONG MUỐN: " + ex.Message.ToString());
                throw new NotImplementedException();
            }
        }

        private static string GenerateGetObjectQuery<T>() where T : new()
        {
            return "SELECT * FROM " + typeof(T).Name + " WHERE Id = @Id";
        }
    }
}