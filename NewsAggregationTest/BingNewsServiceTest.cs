using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.BusinessLogicLayer.Services;
using BingNew.DataAccessLayer.Entities;
using BingNew.ORM.DbContext;
using Moq;

namespace NewsAggregationTest
{
    public class BingNewsServiceTest
    {
        private readonly Mock<IBingNewsService> _bingNewsService = new();
        private readonly DbBingNewsContext _dataContext = new();
  

        [Fact]
        public void GetAllArrticle()
        {
            var result = _dataContext.GetAll<Article>();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
        [Fact]
        public void GetArticleByQuantity()
        {
            var result = _dataContext.GetAll<Article>().Take(5);
            Assert.Equal(5, result.ToList().Count);
        }
        [Fact]
        public void GetAllArticleByQuantityByMock()
        {
            // Arrange
            var articles = new List<Article>
            {
                new Article { Id = Guid.NewGuid(), Title = "Article 1" },
                new Article { Id = Guid.NewGuid(), Title = "Article 2" },
                new Article { Id = Guid.NewGuid(), Title = "Article 3" },
                new Article { Id = Guid.NewGuid(), Title = "Article 4" },
                new Article { Id = Guid.NewGuid(), Title = "Article 5" }
            };
            _bingNewsService.Setup(bingNews => bingNews.GetTrendingArticlesPanel(5)).Returns(articles);

            // Act
            var result = _bingNewsService.Object.GetTrendingArticlesPanel(5);
            
            Assert.NotEmpty(result);
            Assert.Equal(5, result.Count);
        }
        [Fact]
        public void GetTrendingArticlesPanel()
        {
            var bingService = new BingNewsService(_dataContext);
            var articleTrend = bingService.GetTrendingArticlesPanel(12);
            Assert.NotEmpty(articleTrend);

        }

     

    }
}
