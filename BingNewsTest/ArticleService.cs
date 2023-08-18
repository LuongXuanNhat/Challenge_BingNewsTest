using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BingNewsTest
{
    public class ArticleService : IArticle
    {
        public ArticleService() { }

        public PageResult<Article> GetArticles(Pagination pagination)
        {
            var pageResult = new PageResult<Article>(pagination);
            var articles = new List<Article>()
            {
                new Article("Icon1", "Provider1", DateTime.Now, "Article 1", "image1.jpg", 100, 20, 50),
                new Article("Icon2", "Provider2", DateTime.Now, "Article 2", "image2.jpg", 150, 10, 30),
                new Article("Icon3", "Provider1", DateTime.Now, "Article 3", "image3.jpg", 120, 25, 40),
                new Article("Icon4", "Provider3", DateTime.Now, "Article 4", "image4.jpg", 80, 5, 10),
                new Article("Icon5", "Provider2", DateTime.Now, "Article 5", "image5.jpg", 200, 15, 60),
                new Article("Icon6", "Provider3", DateTime.Now, "Article 6", "image6.jpg", 90, 10, 25),
                new Article("Icon7", "Provider1", DateTime.Now, "Article 7", "image7.jpg", 180, 12, 35),
                new Article("Icon8", "Provider2", DateTime.Now, "Article 8", "image8.jpg", 120, 8, 20),
                new Article("Icon9", "Provider1", DateTime.Now, "Article 9", "image9.jpg", 160, 18, 45),
                new Article("Icon10", "Provider3", DateTime.Now, "Article 10", "image10.jpg", 140, 14, 30)
            };
            var data = articles.Skip((pagination.GetIndex() - 1) * pagination.GetSize())
            .Take(pagination.GetSize()).ToList();
            pageResult.Items = data;
            return pageResult;
        }
    }
}
