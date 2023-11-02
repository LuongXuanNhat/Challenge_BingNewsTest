using BingNew.DataAccessLayer.Constants;
using Dasync.Collections;
using System.Data.SqlClient;
using BingNew.ORM.Query;
using BingNew.ORM.NonQuery;
using BingNew.DataAccessLayer.Entities;
using AutoFixture;

namespace NewsAggregationTest
{
    public class ORMTest
    {
        private readonly string _connecString = new ConstantCommon().connectString;
        private readonly Fixture _fixture = new();

        #region Query Single Row
        [Fact]
        public void GetArticle_Success_Using_QuerySingleT()
        {
            using var connection = new SqlConnection(_connecString);
            var sql = "SELECT * FROM Article where Id = '65757875-26e6-4709-aacc-eba7ba4047c4'";
            var result = connection.QuerySingle<Article>(sql);
            Assert.NotNull(result);
        }
        [Fact]
        public void Get_Article_Success_Using_QuerySingle()
        {
            using var connection = new SqlConnection(_connecString);
            var sql = "SELECT * FROM Article where Id = '65757875-26e6-4709-aacc-eba7ba4047c4'";
            var result = connection.QuerySingle(sql);
            Assert.NotNull(result);
            Assert.IsType<Article>(result);
        }
        [Fact]
        public void Get_Article_Return_Null_Using_QuerySingleOrDefault()
        {
            using var connection = new SqlConnection(_connecString);
            var sql = "SELECT * FROM Article where Id = 'dfdab4e6-538b-4ef5-8b69-00f99a9ad6bb'";
            var result = connection.QuerySingleOrDefault(sql);
            Assert.Null(result);
        }
        [Fact]
        public void Get_Article_Return_Error_When_More_Than_One_Element()
        {
            using var connection = new SqlConnection(_connecString);
            var sql = "SELECT * FROM Article where ChannelName = '2'";
            Assert.Throws<InvalidOperationException>(() => connection.QuerySingleOrDefault(sql));
        }
        [Fact]
        public void Get_Article_Return_Null_Using_QuerySingleOrDefaultT()
        {
            using var connection = new SqlConnection(_connecString);
            var sql = "SELECT * FROM Article where Id = 'dfdab4e6-538b-4ef5-8b69-00f99a9ad6bb'";
            var result = connection.QuerySingleOrDefault<Article>(sql);
            Assert.Null(result);
        }
        [Fact]
        public void Get_Article_Error_Return_Null_When_More_Than_One_Element()
        {
            using var connection = new SqlConnection(_connecString);
            var sql = "SELECT * FROM Article where ChannelName = '2'";
            Assert.Throws<InvalidOperationException>(() => connection.QuerySingleOrDefault<Article>(sql));
        }
        [Fact]
        public void Get_Article_Error_Return_Exception_When_More_Than_One_Element_2()
        {
            using var connection = new SqlConnection(_connecString);
            var sql = "SELECT * FROM Article where ChannelName = '12345'";
            Assert.Throws<InvalidOperationException>(() => connection.QuerySingleOrDefault(sql));
        }
        [Fact]
        public void Get_Article_Return_Null_When_Not_Found()
        {
            using var connection = new SqlConnection(_connecString);
            var sql = "SELECT * FROM Article where ChannelName = '22'";
            var result = connection.QuerySingleOrDefault(sql) ;
            Assert.Null(result);
        }
        [Fact]
        public void Get_Article_First_Success()
        {
            using var connection = new SqlConnection(_connecString);
            var sql = "SELECT * FROM Article where ChannelName = '2'";
            var result = connection.QueryFirst(sql);

            Assert.NotNull(result);
            Assert.IsType<Article>(result);
        } 
        [Fact]
        public void Get_Article_First_SuccessT()
        {
            using var connection = new SqlConnection(_connecString);
            var sql = "SELECT * FROM Article where ChannelName = '2'";
            var result = connection.QueryFirst<Article>(sql);

            Assert.NotNull(result);
            Assert.IsType<Article>(result);
        }

        [Fact]
        public void Get_Article_First_Error_When_Not_Found()
        {
            using var connection = new SqlConnection(_connecString);
            var sql = "SELECT * FROM Article where ChannelName = '1'";
            Assert.Throws<InvalidOperationException>(() => connection.QueryFirst(sql));
        }

        [Fact]
        public void Get_Article_First_Error_When_Not_FoundT()
        {
            using var connection = new SqlConnection(_connecString);
            var query = "SELECT * FROM Article where ChannelName = '1'";
            Assert.Throws<InvalidOperationException>(() => connection.QueryFirst<Article>(query));
        }

        [Fact]
        public void Get_Article_First_Success_Using_QueryFirstOrDefault()
        {
            using var connection = new SqlConnection(_connecString);
            var sql = "SELECT * FROM Article where ChannelName = '2'";
            var result = connection.QueryFirstOrDefault(sql);

            Assert.NotNull(result);
            Assert.IsType<Article>(result);
        }
        
        [Fact]
        public void Get_Article_First_Return_Null_Using_QueryFirstOrDefault()
        {
            using var connection = new SqlConnection(_connecString);
            var sql = "SELECT * FROM Article where ChannelName = '1'";
            var result = connection.QueryFirstOrDefault(sql);

            Assert.Null(result);
        }

        [Fact]
        public void Get_Article_First_Success_Using_QueryFirstOrDefaultT()
        {
            using var connection = new SqlConnection(_connecString);
            var sql = "SELECT * FROM Article where ChannelName = '2'";
            var result = connection.QueryFirstOrDefault<Article>(sql);

            Assert.NotNull(result);
            Assert.IsType<Article>(result);
        }
        
        [Fact]
        public void Get_Article_First_Return_Null_Using_QueryFirstOrDefaultT()
        {
            using var connection = new SqlConnection(_connecString);
            var sql = "SELECT * FROM Article where ChannelName = '1'";
            var result = connection.QueryFirstOrDefault<Article>(sql);

            Assert.Null(result);
        }


        #endregion

        #region Query Scalar Values 

        [Fact]
        public void Get_Total_Article_Success()
        {
            using var connection = new SqlConnection(_connecString);
            var sql = "SELECT COUNT(*) FROM Article";
            var count = connection.ExecuteScalar(sql);

            Assert.NotNull(count);
            Assert.True(count > 0);
        }
        [Fact]
        public async Task Get_Total_Article_Success_Using_ExecuteScalarAsync()
        {
            using var connection = new SqlConnection(_connecString);
            var sql = "SELECT COUNT(*) FROM Article";
            var count = await connection.ExecuteScalarAsync(sql);

            Assert.NotNull(count);
            Assert.True(count > 0);
        }
        [Fact]
        public void Get_Total_Article_SuccessT()
        {
            using SqlConnection connection = new(_connecString);
            var sql = "SELECT COUNT(*) FROM Article";
            var count = connection.ExecuteScalar<int>(sql);

            Assert.True(count > 0);
        }
        [Fact]
        public async Task Get_Total_Article_Success_Using_ExecuteScalarAsyncT()
        {
            using SqlConnection connection = new(_connecString);
            var sql = "SELECT COUNT(*) FROM Article";
            var count = await connection.ExecuteScalarAsync<int>(sql);

            Assert.True(count > 0);
        }

        #endregion

        #region Query Multiple Rows

        [Fact]
        public void Get_Articles_Success()
        {
            using var connection = new SqlConnection(_connecString);
            var sql = "SELECT * FROM Article WHERE ChannelName = '2';";
            var result = connection.Query(sql).ToList();
            var firstArticle = result.FirstOrDefault() as Article;

            Assert.NotEmpty(result);
            Assert.NotNull(firstArticle?.Title);
        }

        [Fact]
        public void Get_Articles_SuccessT()
        {
            using var connection = new SqlConnection(_connecString);
            var sql = "SELECT * FROM Article";
            var result = connection.Query<Article>(sql).ToList();
            var first = result.FirstOrDefault();

            Assert.NotEmpty(result);
            Assert.IsType<Article>(first);
        }
        [Fact]
        public void Get_Articles_SuccessT2()
        {
            using var connection = new SqlConnection(_connecString);
            var sql = "SELECT * FROM Weather";
            var result = connection.Query<Weather>(sql).ToList();
            var first = result.FirstOrDefault();

            Assert.NotEmpty(result);
            Assert.IsType<Weather>(first);
        }

        [Fact]
        public async Task Get_Articles_Async_Success()
        {
            using var connection = new SqlConnection(_connecString);
            var sql = "SELECT * FROM Article";
            var result = await connection.QueryAsync(sql).ToListAsync();

            Assert.NotEmpty(result);
            Assert.IsType<Article>(result.FirstOrDefault());
        }

        [Fact]
        public async Task Get_Articles_AsyncT_Success()
        {
            using var connection = new SqlConnection(_connecString);
            var sql = "SELECT * FROM Article";
            var result = await connection.QueryAsync<Article>(sql).ToListAsync();

            Assert.NotEmpty(result);
            Assert.IsType<Article>(result.FirstOrDefault());
        }

        #endregion

        #region Query Multiple Results

        [Fact]
        public void Test_QueryMultiple()
        {
            using var connection = new SqlConnection(_connecString);
            string sql = @"
                SELECT * FROM Article WHERE ChannelName = '2';
                SELECT * FROM Provider WHERE Id = '2';
            ";
            var result = connection.QueryMultiple(sql).ToList();
            var firstArticle = result.FirstOrDefault() as Article;
            connection.Close();
            Assert.NotEmpty(result);
            Assert.NotNull(firstArticle?.Title);
        }
        
        [Fact]
        public async Task Test_QueryMultipleAsync()
        {
            using var connection = new SqlConnection(_connecString);
            string sql = @"
                SELECT * FROM Article WHERE ChannelName = '2';
                SELECT * FROM Provider WHERE Id = '2';      
                ";
            var result = await connection.QueryMultipleAsync(sql);
            var firstArticle = await result.FirstOrDefaultAsync() as Article;

            Assert.NotNull(result);
            Assert.NotNull(firstArticle?.Title);
        }

        [Fact]
        public void Read_QueryMultiple_Success()
        {
            using var connection = new SqlConnection(_connecString);
            string sql = @"
                SELECT * FROM Article WHERE ChannelName = '2';
                SELECT * FROM Provider WHERE Id = '2';      
                ";
            var result = connection.QueryMultiple(sql);

            var articles = result.Read<Article>().ToList();
            Assert.Equal(3, articles.Count);

            var providers = result.Read<Provider>().ToList();
            Assert.NotNull(providers);
        }

        [Fact]
        public void ReadFirst_QueryMultiple_Success()
        {
            using var connection = new SqlConnection(_connecString);
            string sql = @"
                SELECT * FROM Article WHERE ChannelName = '2';
                SELECT * FROM Provider WHERE Id = '2';      
                ";
            var result = connection.QueryMultiple(sql);

            var article = result.ReadFirst<Article>();
            Assert.NotNull(article);
            Assert.IsType<Article>(article);

            var provider = result.ReadFirst<Provider>();
            Assert.NotNull(provider);
        }

        [Fact]
        public async Task ReadAsync_QueryMultiple_Success()
        {
            using var connection = new SqlConnection(_connecString);
            string sql = @"
                SELECT * FROM Article WHERE ChannelName = '2';
                SELECT * FROM Provider WHERE Id = '2';      
                ";
            var result = await connection.QueryMultipleAsync(sql);

            var articles = await result.ReadAsync<Article>().ToListAsync();
            var firstArticle = articles.FirstOrDefault();

            Assert.NotNull(articles);
            Assert.NotNull(firstArticle?.Title);
        }

        [Fact]
        public async Task ReadFirstAsync_QueryMultiple_Success()
        {
            using var connection = new SqlConnection(_connecString);
            string sql = @"
                SELECT * FROM Article WHERE ChannelName = '2';
                SELECT * FROM Provider WHERE Id = '2'; ";
            var result = await connection.QueryMultipleAsync(sql);

            var article = await result.ReadFirstAsync<Article>();
            Assert.NotNull(article);

            var provider = await result.ReadFirstAsync<Topic>();
            Assert.Null(provider);
        }

        #endregion

        #region Non-Query with CRUD


        [Fact]
        public void Object_Insert_Success()
        {
            var article = _fixture.Create<Article>();
            article.PubDate = DateTime.Now;
            using var connection = new SqlConnection(_connecString);
            bool result = connection.Insert(article);
            Assert.True(result);
        }
        [Fact]
        public void Object_GetById_Success()
        {
            using var connection = new SqlConnection(_connecString);
            Guid articleId = new("173341eb-34db-4a3b-b93d-fe5cf6872a2f");
            var article = connection.GetById<Article>(articleId);

            Assert.NotNull(article);
            Assert.Equal(articleId, article.Id);
        }

        [Fact]
        public void Object_Update_Success()
        {
            using var connection = new SqlConnection(_connecString);
            Guid articleId = new("173341eb-34db-4a3b-b93d-fe5cf6872a2f");
            var article = connection.GetById<Article>(articleId);

            Assert.NotNull(article);
            article.Title = "Updated Titleee";

            bool result = connection.Update(article);
            Assert.True(result);

            var updatedArticle = connection.GetById<Article>(article.Id);
            Assert.Equal("Updated Titleee", updatedArticle?.Title);
        }

        [Fact]
        public void Object_Delete_Success()
        {
            var article = new Article()
            {
                CommentNumber = 1,
                Description = "Test",
                 DisLikeNumber = 2,
                ImgUrl = null,
                Id = Guid.NewGuid(),
                LikeNumber = 3,
                ChannelName = Guid.NewGuid().ToString(),
                PubDate = DateTime.Now,
                Title = "Test",
                Url = null,
                ViewNumber = 1,
                TopicId = ""
            };

            using var connection = new SqlConnection(_connecString);
            connection.Insert(article);
            bool result = connection.Delete<Article>(article.Id);
            Assert.True(result);

            var deletedArticle = connection.GetById<Article>(article.Id);
            Assert.Null(deletedArticle);
        }
        #endregion
    }

}
