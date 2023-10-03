using BingNew.DataAccessLayer.Constants;
using BingNew.DataAccessLayer.Entities;
using BingNew.ORM.Query;
using System.Data;
using System.Data.SqlClient;

namespace BingNew.ORM.DbContext
{
    public sealed class DbBingNewsContext
    {
        private readonly ConstantCommon _constant;
        public DbBingNewsContext()
        {
            _constant = new ConstantCommon();
        }
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_constant.connectString);
        }

        public List<T> GetAll<T>() where T : class
        {
            using var connection = CreateConnection();
            var tableName = typeof(T).Name;
            var sql = "SELECT * FROM " + tableName;
            return connection.Query<T>(sql).ToList();
        }
    }
}
