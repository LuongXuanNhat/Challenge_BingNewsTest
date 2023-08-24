using BingNew.DataAccessLayer.Models;

public interface IDataSource
{
    public List<Article> GetNews(Config config);
}