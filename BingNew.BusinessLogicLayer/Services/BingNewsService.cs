using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.DataAccessLayer.Entities;
using BingNew.ORM.DbContext;

namespace BingNew.BusinessLogicLayer.Services
{
    public class BingNewsService : IBingNewsService
    {
        private readonly DbBingNewsContext _dataContext;
        public BingNewsService(DbBingNewsContext context) {
            _dataContext = context;
        }

        public List<Article> GetTopNews(int quantity)
        {
            var a = _dataContext.GetAll<Article>().OrderBy(x=>x.PubDate).ToList();
            var articles = _dataContext.GetAll<Article>()
                            .Where(x => x.PubDate == DateTime.Now)
                            .OrderBy(x => x.LikeNumber + x.ViewNumber + x.CommentNumber*2 + x.DisLikeNumber)
                            .Take(quantity)
                            .ToList();
            return articles;
        }

        public List<Article> GetTrendingArticlesPanel(int quantity)
        {
            var articles = _dataContext.GetAll<Article>()
                            .Where(x => x.PubDate >= DateTime.Now.AddDays(-3))
                            .Take(quantity)
                            .ToList();

            return articles;
        }

       
    }
}