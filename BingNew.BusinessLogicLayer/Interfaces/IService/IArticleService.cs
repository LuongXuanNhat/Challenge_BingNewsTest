using BingNew.BusinessLogicLayer.ModelConfig;
using BingNew.DataAccessLayer.Models;

namespace BingNew.BusinessLogicLayer.Interfaces.IService
{
    public interface IArticleService : IBaseService<Article>
    {
        Task<bool> AddRange(IEnumerable<Article> articles);
        Task<List<Article>> UpdateArticlesFromTuoiTreNews(Config config);
      ////  Task<List<Article>> TrendingStories();
    }
}
