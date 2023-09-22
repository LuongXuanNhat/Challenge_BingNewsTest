using BingNew.BusinessLogicLayer.Services.Common;
using BingNew.DataAccessLayer.Constants;
using BingNew.DataAccessLayer.Models;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace NewsAggregationTest
{
    public class ORMTest
    {
        private readonly string _connecString;
        public ORMTest()
        {
            _connecString = new ConstantCommon().connectString;
        }

        #region ORM
        [Fact]
        public void GetArticle_Success_Using_QuerySingleT()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                var sql = "SELECT * FROM Article where Id = 'dfdab4e6-538b-4ef5-8b69-00f99a9ad6bf'";
                var result = connection.QuerySingle<Article>(sql);
                Assert.NotNull(result);
            }
        }
        [Fact]
        public void Get_Article_Success_Using_QuerySingle()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                connection.Open();
                var sql = "SELECT * FROM Article where Id = 'dfdab4e6-538b-4ef5-8b69-00f99a9ad6bf'";
                var result = connection.QuerySingle(sql);
                Assert.NotNull(result);
                Assert.IsType<Article>(result);
            }
        }

        [Fact]
        public void Get_Article_Return_Null_Using_QuerySingleOrDefault()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                connection.Open();
                var sql = "SELECT * FROM Article where Id = 'dfdab4e6-538b-4ef5-8b69-00f99a9ad6bb'";
                var result = connection.QuerySingleOrDefault(sql);
                Assert.Null(result);
            }
        }
        [Fact]
        public void Get_Article_Return_Null_Using_QuerySingleOrDefaultT()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                connection.Open();
                var sql = "SELECT * FROM Article where Id = 'dfdab4e6-538b-4ef5-8b69-00f99a9ad6bb'";
                var result = connection.QuerySingleOrDefault<Article>(sql);
                Assert.Null(result);
            }
        }
        [Fact]
        public void Get_Article_Error_Return_Exception_When_More_Than_One_Element()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                connection.Open();
                var sql = "SELECT * FROM Article where ProviderId = 2";
                Assert.Throws<InvalidOperationException>(() =>
                {
                    connection.QuerySingleOrDefault<Article>(sql);
                });
            }
        }
        [Fact]
        public void Get_Article_Error_Return_Exception_When_More_Than_One_Element_2()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                connection.Open();
                var sql = "SELECT * FROM Article where ProviderId = 2";
                Assert.Throws<InvalidOperationException>(() =>
                {
                    connection.QuerySingleOrDefault(sql);
                });
            }
        }
        [Fact]
        public void Get_Article_First_Success()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                connection.Open();
                var sql = "SELECT * FROM Article where ProviderId = 2";
                var result = connection.QueryFirst<Article>(sql);

                Assert.NotNull(result);
                Assert.IsType<Article>(result);
            }
        }
        [Fact]
        public void Get_Article_First_Error_When_Not_Found()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                connection.Open();
                var sql = "SELECT * FROM Article where ProviderId = 1";
                Assert.Throws<InvalidOperationException>(() =>
                {
                    connection.QueryFirst<Article>(sql);
                });
            }
        }

        #endregion
    }

}
