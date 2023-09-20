using BingNew.BusinessLogicLayer.DapperContext;
using BingNew.BusinessLogicLayer.Interfaces.IRepository;
using BingNew.DataAccessLayer.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;

namespace BingNew.DataAccessLayer.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly IDbConnection _dbConnection;
        public ArticleRepository() {
            _dbConnection = new DbContext().CreateConnection();
        }
        public async Task Add(Article article)
        {
            _dbConnection.Open();
            string query = "INSERT INTO Article (Id, Title, ImgUrl, Description, PubDate, Url, LikeNumber, DisLikeNumber, ViewNumber, CommentNumber) " +
                 "VALUES (@Id, @Title, @ImgUrl, @Description, @PubDate, @Url, @LikeNumber, @DisLikeNumber, @ViewNumber, @CommentNumber)";

            await _dbConnection.ExecuteAsync(query, article);
            _dbConnection.Close();
        }

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

        //    //using (var connection = new SqlConnection(_constant.connectString))
        //    //{
        //    //    connection.Open();
        //    //    await connection.ExecuteAsync(query, entity);
        //    //}

        //    _dbConnection.Open();
        //    await _dbConnection.ExecuteAsync(query, entity);
        //    _dbConnection.Close();
        //}

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

        public async Task<bool> Update(Article article)
        {
            _dbConnection.Open();
            string selectQuery = $@"SELECT * FROM Article WHERE id = '{article.GetId()}'";
            var entity = await _dbConnection.QueryAsync<Article>(selectQuery, article.GetId());
            if (entity is null)
                return false;

            string query = "UPDATE Article SET Title = @Title, ImgUrl = @ImgUrl, Description = @Description, PubDate = @PubDate, " +
                "Url = @Url, LikeNumber = @LikeNumber, DisLikeNumber = @DisLikeNumber, ViewNumber = @ViewNumber, CommentNumber = @CommentNumber " +
                "WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(query, article);
            _dbConnection.Close();
            return true;
        }

    }
}
