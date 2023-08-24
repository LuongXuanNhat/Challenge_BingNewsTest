using BingNew.DataAccessLayer.Models;

public interface ITypeRssSource
{
    public List<Article> GetArticles(Config config);
}