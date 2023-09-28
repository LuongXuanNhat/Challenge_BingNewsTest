using BingNew.BusinessLogicLayer.ModelConfig;
using BingNew.DataAccessLayer.Models;

namespace BingNew.BusinessLogicLayer.Interfaces.IService
{
    public interface IArticleService : IBaseService<ArticleVm>
    {
        Task<bool> AddRange(IEnumerable<ArticleVm> articles);
        Task<List<ArticleVm>> UpdateArticlesFromTuoiTreNews(Config config);
      ////  Task<List<ArticleVm>> TrendingStories();
    }
}
