using BingNew.BusinessLogicLayer;
using BingNew.BusinessLogicLayer.Interfaces;
using BingNew.DataAccessLayer.Models;
using Dapper;
using System.Data;

namespace BingNew.DataAccessLayer.Repositories
{
    public class ArticleRepository : IBaseRepository<Article> 
    {
        private readonly IDbConnection _dbConnection;
        public ArticleRepository(DbContext dbContext) {
            _dbConnection = dbContext.CreateConnection();
        }
        public async Task Add(Article article)
        {
            _dbConnection.Open();
            string query = "INSERT INTO Article (Id, Title, ImgUrl, Description, PubDate, Url, LikeNumber, DisLikeNumber, ViewNumber, CommentNumber) " +
                 "VALUES (@Id, @Title, @ImgUrl, @Description, @PubDate, @Url, @LikeNumber, @DisLikeNumber, @ViewNumber, @CommentNumber)";

            _dbConnection.Execute(query, article);
            _dbConnection.Close();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Article> GetAll()
        {
            _dbConnection.Open();
            string query = "SELECT * FROM Article";
            var result = _dbConnection.Query<Article>(query);
            _dbConnection.Close();
            return result;
        }

        public void GetById(string id)
        {
            throw new NotImplementedException();
        }

        public void Update(Article entity)
        {
            throw new NotImplementedException();
        }
    }
}
