using BingNew.BusinessLogicLayer.Query;
using BingNew.DataAccessLayer.Constants;
using BingNew.DataAccessLayer.Models;
using Dasync.Collections;
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

        #region Query Single Row
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
                var sql = "SELECT * FROM Article WHERE ProviderId = '2';";
                var result = connection.Query(sql).ToList();
                var firstArticle = result.FirstOrDefault() as Article;

                Assert.NotEmpty(result);
                if (firstArticle != null)
                {
                    Assert.NotNull(firstArticle.GetTitle());
                } else Assert.True(false);
            }
        }

        [Fact]
        public void Get_Articles_SuccessT()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                var sql = "SELECT * FROM Article";
                var result = connection.Query<Article>(sql).ToList();
                var first = result.FirstOrDefault();

                Assert.NotEmpty(result);
                Assert.IsType<Article>(first);
            }
        }

        [Fact]
        public async Task Get_Articles_Async_Success()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                var sql = "SELECT * FROM Article";
                var result = await connection.QueryAsync(sql).ToListAsync();

                Assert.NotEmpty(result);
                Assert.IsType<Article>(result.FirstOrDefault());
            }
        }

        [Fact]
        public async Task Get_Articles_AsyncT_Success()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                var sql = "SELECT * FROM Article";
                var result = await connection.QueryAsync<Article>(sql).ToListAsync();

                Assert.NotEmpty(result);
                Assert.IsType<Article>(result.FirstOrDefault());
            }
        }

        #endregion

        #region Query Multiple Results

        [Fact]
        public void Test_QueryMultiple()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                string sql = @"
                SELECT * FROM Article WHERE ProviderId = '2';
                SELECT * FROM Provider WHERE Id = '2';
            ";
                var result = connection.QueryMultiple(sql).ToList();
                var firstArticle = result.FirstOrDefault() as Article;

                Assert.NotEmpty(result);
                if (firstArticle != null)
                {
                    Assert.NotNull(firstArticle.GetTitle());
                }
                else Assert.True(false);
            }
        }
        
        [Fact]
        public async Task Test_QueryMultipleAsync()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                string sql = @"
                SELECT * FROM Article WHERE ProviderId = '2';
                SELECT * FROM Provider WHERE Id = '2';      
                ";
                var result = await connection.QueryMultipleAsync(sql);
                var firstArticle = await result.FirstOrDefaultAsync() as Article;

                Assert.NotNull(result);
                if (firstArticle != null)   
                     Assert.NotNull(firstArticle.GetTitle());
                else Assert.True(false);
                
            }
        }

        [Fact]
        public void Read_QueryMultiple_Success()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                string sql = @"
                SELECT * FROM Article WHERE ProviderId = '2';
                SELECT * FROM Provider WHERE Id = '2';      
                ";
                var result = connection.QueryMultiple(sql);

                var articles = result.Read<Article>().ToList();
                Assert.NotNull(articles);

                var providers = result.Read<Provider>().ToList();
                Assert.NotNull(providers);
            }
        }

        [Fact]
        public void ReadFirst_QueryMultiple_Success()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                string sql = @"
                SELECT * FROM Article WHERE ProviderId = '2';
                SELECT * FROM Provider WHERE Id = '2';      
                ";
                var result = connection.QueryMultiple(sql);

                var article = result.ReadFirst<Article>();
                Assert.NotNull(article);
                Assert.IsType<Article>(article);

                var provider = result.ReadFirst<Provider>();
                Assert.NotNull(provider);
            }
        }

        [Fact]
        public async Task ReadAsync_QueryMultiple_Success()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                string sql = @"
                SELECT * FROM Article WHERE ProviderId = '2';
                SELECT * FROM Provider WHERE Id = '2';      
                ";
                var result = await connection.QueryMultipleAsync(sql);

                var articles = await result.ReadAsync<Article>().ToListAsync();
                var firstArticle = articles.FirstOrDefault();

                Assert.NotNull(articles);
                if (firstArticle != null) Assert.NotNull(firstArticle.GetTitle());
                else Assert.True(false);
            }
        }

        [Fact]
        public async Task ReadFirstAsync_QueryMultiple_Success()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                string sql = @"
                SELECT * FROM Article WHERE ProviderId = '2';
                SELECT * FROM Provider WHERE Id = '2'; ";
                var result = await connection.QueryMultipleAsync(sql);

                var article = await result.ReadFirstAsync<Article>();
                Assert.NotNull(article);

                var provider = await result.ReadFirstAsync<Topic>();
                Assert.Null(provider);
            }
        }

        #endregion

        #region Non-Query with CRUD

        [Fact]
        public void Object_Insert_Success()
        {

        }

        #endregion
    }

}
