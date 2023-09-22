using BingNew.BusinessLogicLayer.Query;
using BingNew.DataAccessLayer.Constants;
using BingNew.DataAccessLayer.Models;
using System.Data.SqlClient;

namespace NewsAggregationTest
{
    public class ORMTest
    {
        private readonly string _connecString;
        public ORMTest()
        {
            _connecString = new ConstantCommon().connectString;
        }

        #region ORM Query Single Row
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
                var sql = "SELECT * FROM Article where ProviderId = 2";
                var result = connection.QueryFirst(sql);

                Assert.NotNull(result);
                Assert.IsType<Article>(result);
            }
        } 
        [Fact]
        public void Get_Article_First_SuccessT()
        {
            using (var connection = new SqlConnection(_connecString))
            {
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
                var sql = "SELECT * FROM Article where ProviderId = 1";
                Assert.Throws<InvalidOperationException>(() =>
                {
                    connection.QueryFirst(sql);
                });
            }
        }

        [Fact]
        public void Get_Article_First_Error_When_Not_FoundT()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                var sql = "SELECT * FROM Article where ProviderId = 1";
                Assert.Throws<InvalidOperationException>(() =>
                {
                    connection.QueryFirst<Article>(sql);
                });
            }
        }

        [Fact]
        public void Get_Article_First_Success_Using_QueryFirstOrDefault()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                var sql = "SELECT * FROM Article where ProviderId = 2";
                var result = connection.QueryFirstOrDefault(sql);

                Assert.NotNull(result);
                Assert.IsType<Article>(result);
            }
        }
        
        [Fact]
        public void Get_Article_First_Return_Null_Using_QueryFirstOrDefault()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                var sql = "SELECT * FROM Article where ProviderId = 1";
                var result = connection.QueryFirstOrDefault(sql);

                Assert.Null(result);
            }
        }

        [Fact]
        public void Get_Article_First_Success_Using_QueryFirstOrDefaultT()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                var sql = "SELECT * FROM Article where ProviderId = 2";
                var result = connection.QueryFirstOrDefault<Article>(sql);

                Assert.NotNull(result);
                Assert.IsType<Article>(result);
            }
        }
        
        [Fact]
        public void Get_Article_First_Return_Null_Using_QueryFirstOrDefaultT()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                var sql = "SELECT * FROM Article where ProviderId = 1";
                var result = connection.QueryFirstOrDefault<Article>(sql);

                Assert.Null(result);
            }
        }


        #endregion

        #region Query Scalar Values 

        [Fact]
        public void Get_Total_Article_Success()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                var sql = "SELECT COUNT(*) FROM Article";
                var count = connection.ExecuteScalar(sql);

                Assert.NotNull(count);
                Assert.True(count > 0);
            }
        }
        [Fact]
        public async Task Get_Total_Article_Success_Using_ExecuteScalarAsync()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                var sql = "SELECT COUNT(*) FROM Article";
                var count = await connection.ExecuteScalarAsync(sql);

                Assert.NotNull(count);
                Assert.True(count > 0);
            }
        }
        [Fact]
        public void Get_Total_Article_SuccessT()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                var sql = "SELECT COUNT(*) FROM Article";
                var count = connection.ExecuteScalar<int>(sql);

                Assert.True(count > 0);
            }
        }
        [Fact]
        public async Task Get_Total_Article_Success_Using_ExecuteScalarAsyncT()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                var sql = "SELECT COUNT(*) FROM Article";
                var count = await connection.ExecuteScalarAsync<int>(sql);

                Assert.True(count > 0);
            }
        }

        #endregion

        #region Query Multiple Rows

        [Fact]
        public void Get_Articles_Success()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                var sql = "SELECT * FROM Article";
                var result = connection.Query(sql).ToList();

                Assert.NotEmpty(result);
                Assert.IsType<Article>(result.First());
            }
        }

        [Fact]
        public void Get_Articles_SuccessT()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                var sql = "SELECT * FROM Article";
                var result = connection.Query<Article>(sql).ToList();
                var first = result.First();

                Assert.NotEmpty(result);
                Assert.IsType<Article>(first);
            }
        }


        #endregion
    }

}
