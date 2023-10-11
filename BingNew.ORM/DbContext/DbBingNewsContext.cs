using BingNew.DataAccessLayer.Constants;
using BingNew.ORM.NonQuery;
using BingNew.ORM.Query;
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

        public void Add<T>(T weather)
        {
            SqlExtensionNonQuery.Insert<T>(CreateConnection() ,weather);
        }

        public void AddRanger<T>(List<T> result)
        {
            foreach (T item in result)
            {
                SqlExtensionNonQuery.Insert<T>(CreateConnection(), item);
            }
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
