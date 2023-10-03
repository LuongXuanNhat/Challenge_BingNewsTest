using BingNew.DataAccessLayer.Entities;

namespace BingNew.BusinessLogicLayer.Interfaces.IService
{
    public interface IBingNewsService
    {
        List<Article> GetTrendingArticlesPanel(int articleNumber);
    }
}