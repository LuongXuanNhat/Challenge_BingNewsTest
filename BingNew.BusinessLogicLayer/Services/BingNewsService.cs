using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.DataAccessLayer.Entities;
using BingNew.ORM.DbContext;

namespace NewsAggregationTest
{
    public class BingNewsService : IBingNewsService
    {
        private readonly DbBingNewsContext _dataContext;
        public BingNewsService(DbBingNewsContext context) {
            _dataContext = context;
        }

        public List<Article> GetTrendingArticlesPanel(int articleNumber)
        {
            var articles = _dataContext.GetAll<Article>()
                            .Where(x => x.PubDate >= DateTime.Now.AddDays(-3))
                            .Take(articleNumber)
                            .ToList();

            return articles;
        }
    }
}