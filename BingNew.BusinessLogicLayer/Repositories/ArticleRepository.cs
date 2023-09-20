using BingNew.BusinessLogicLayer;
using BingNew.BusinessLogicLayer.Interfaces.IRepository;
using BingNew.DataAccessLayer.Models;
using Dapper;
using System.Data;

namespace BingNew.DataAccessLayer.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly IDbConnection _dbConnection;
        public ArticleRepository() {
            _dbConnection = new DapperContext().CreateConnection();
        }
        public async Task Add(Article article)
        {
            _dbConnection.Open();
            string query = "INSERT INTO Article (Id, Title, ImgUrl, Description, PubDate, Url, LikeNumber, DisLikeNumber, ViewNumber, CommentNumber) " +
                 "VALUES (@Id, @Title, @ImgUrl, @Description, @PubDate, @Url, @LikeNumber, @DisLikeNumber, @ViewNumber, @CommentNumber)";

            await _dbConnection.ExecuteAsync(query, article);
            _dbConnection.Close();
        }

        public async Task Delete(string id)
        {
            _dbConnection.Open();
            string query = "DELETE FROM Article WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(query, new { Id = id });
            _dbConnection.Close();
        }

        public async Task<IEnumerable<Article>> GetAll()
        {
            _dbConnection.Open();
            string query = "SELECT * FROM Article";
            var result = await _dbConnection.QueryAsync<Article>(query);
            _dbConnection.Close();
            return result;
        }

        public async Task<Article> GetById(string id)
        {
            _dbConnection.Open();
            string query = "SELECT * FROM Article WHERE Id = @Id";
            var result = await _dbConnection.QueryFirstOrDefaultAsync<Article>(query, new { Id = id });
            _dbConnection.Close();
            return result;
        }

        public async Task Update(Article article)
        {
            _dbConnection.Open();
            string query = "UPDATE Article SET Title = @Title, ImgUrl = @ImgUrl, Description = @Description, PubDate = @PubDate, " +
                "Url = @Url, LikeNumber = @LikeNumber, DisLikeNumber = @DisLikeNumber, ViewNumber = @ViewNumber, CommentNumber = @CommentNumber " +
                "WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(query, article);
            _dbConnection.Close();
        }
    }
}
