﻿using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;

namespace BingNew.ORM.NonQuery
{
    public static class SqlExtensionNonQuery
    {
        public static bool Insert<T>(this SqlConnection connection,T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));

                }
                var sql = GenerateInsertQuery(entity);
                if (connection.State == ConnectionState.Closed) connection.Open();
                using (var command = new SqlCommand(sql, connection))
                {
                    foreach (var property in typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        var paramName = "@" + property.Name;
                        var value = property.GetValue(entity);
                        command.Parameters.AddWithValue(paramName, value ?? DBNull.Value);
                    }

                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("-------------------------------------------   XUẤT HIỆN LỖI KHÔNG MONG MUỐN: " + ex.Message.ToString());
                return false;
            }
        }

        public static string GenerateInsertQuery<T>(T entity)
        {
            string tableName = typeof(T).Name;
            FieldInfo[] properties = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
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
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }
                var sql = GenerateUpdateQuery(entity);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (var command = new SqlCommand(sql, connection))
                {
                    foreach (var property in typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        var paramName = "@" + property.Name;
                        var value = property.GetValue(entity);
                        command.Parameters.AddWithValue(paramName, value ?? DBNull.Value);
                    }

                    command.ExecuteNonQuery();
                }

                return true;
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
            FieldInfo[] properties = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            string setClause = string.Join(", ", properties.Select(p => p.Name + " = @" + p.Name));
            string query = $"UPDATE {tableName} SET {setClause} WHERE Id = @Id";

            return query;
        }

        public static bool Delete<T>(this SqlConnection connection, Guid entityId)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                var tableName = typeof(T).Name;
                var sql = $"DELETE FROM {tableName} WHERE Id = @Id";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", entityId);
                    command.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("XUẤT HIỆN LỖI KHÔNG MONG MUỐN: " + ex.Message.ToString());
                return false;
            }
        }

        public static T GetById<T>(this SqlConnection connection, Guid entityId) where T : new()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                var tableName = typeof(T).Name;
                var sql = $"SELECT * FROM {tableName} WHERE Id = @Id";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", entityId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var entity = new T();
                            foreach (var property in typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                            {
                                var value = reader[property.Name];
                                property.SetValue(entity, value == DBNull.Value ? null : value);
                            }

                            return entity;
                        }
                    }
                }

                return default(T);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("XUẤT HIỆN LỖI KHÔNG MONG MUỐN: " + ex.Message.ToString());
                return default(T);
            }
        }

    }
}