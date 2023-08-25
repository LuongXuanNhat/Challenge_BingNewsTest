using BingNew.DataAccessLayer.Models;

public interface IDataSource
{
    public List<Article> GetArticles(Config config);
}