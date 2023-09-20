using BingNew.BusinessLogicLayer.Interfaces.IRepository;
using BingNew.DataAccessLayer.Constants;
using BingNew.DataAccessLayer.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BingNew.BusinessLogicLayer.DapperContext
{
    public class DbContext
    {
        private readonly ConstantCommon _constant;
        private readonly IDbConnection _dbContext;
        public DbContext()
        {
            _constant = new ConstantCommon();
            _dbContext = CreateConnection();
        }
        #region Connect Database
        public IDbConnection CreateConnection() => new SqlConnection(_constant.connectString);
        #endregion
        //public async Task Add(T entity)
        //{
        //    if (entity == null)
        //    {
        //        throw new ArgumentNullException(nameof(entity));
        //    }

        //    // Tạo câu truy vấn SQL INSERT
        //    string tableName = typeof(T).Name;
        //    PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);

        //    string columns = string.Join(", ", properties.Select(p => p.Name));
        //    string values = string.Join(", ", properties.Select(p => "@" + p.Name));
        //    string query = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

        //    using (var connection = new SqlConnection(_constant.connectString))
        //    {
        //        connection.Open();
        //        await connection.ExecuteAsync(query, entity);
        //    }
        //}

        //private string InsertQuery(T entity)
        //{
        //    FieldInfo[] properties = entity.GetType()
        //        .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        //    if (properties.Length == 0)
        //    {
        //        throw new ArgumentException("Object does not have any properties");
        //    }

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("INSERT INTO ");
        //    sb.Append(entity.GetType().Name);
        //    sb.Append(" (");
        //    foreach (var property in properties)
        //    {
        //        sb.Append(property.Name);
        //        sb.Append(",");
        //    }
        //    sb.Length--;

        //    sb.Append(") VALUES (");
        //    foreach (var property in properties)
        //    {
        //        sb.Append(" @");
        //        sb.Append(property.Name);
        //        sb.Append(",");
        //    }
        //    sb.Length--;
        //    sb.Append(")");
        //    return sb.ToString();
        //}


    }
}
