using AutoFixture;
using BingNew.BusinessLogicLayer;
using BingNew.BusinessLogicLayer.Interfaces;
using BingNew.BusinessLogicLayer.ModelConfig;
using BingNew.BusinessLogicLayer.Services;
using BingNew.BusinessLogicLayer.Services.Common;
using BingNew.DataAccessLayer.Models;
using BingNew.DataAccessLayer.Repositories;
using BingNew.DataAccessLayer.TestData;
using Xunit.Sdk;

namespace NewsAggregationTest
{
    public class BingNewsTest
    {
        private readonly NewsService _newsService;
        private readonly DataSample _dataSample;
        private readonly Config _config;
        private readonly IFixture _fixture;
        private readonly DbContext _dbContext;
        private readonly ArticleService _articleService;
        private readonly ArticleRepository _articleRepository;

        public BingNewsTest()
        {
            _dataSample = new DataSample();
            _newsService = new NewsService();
            _config = new Config();
            _fixture = new Fixture();
            _dbContext = new DbContext();
            _articleRepository = new ArticleRepository(_dbContext);
            _articleService = new ArticleService(_articleRepository);
        }

        [Fact]
        public void CreateNewsServiceNotNull()
        {
            var service = new NewsService();
            Assert.NotNull(service);
        }

        [Fact]
        public void GetNewsFromRssTuoiTreNotNull()
        {
            IDataSource _dataSource = new RssDataSource();
            var result = _dataSource.GetNews("https://tuoitre.vn/rss/tin-moi-nhat.rss");
            Assert.NotNull(result);
        }

        [Fact]
        public void GetNewsFromRssGoogleTrendNotNull()
        {
            IDataSource _dataSource = new RssDataSource();
            var result = _dataSource.GetNews("https://trends.google.com.vn/trends/trendingsearches/daily/rss?geo=VN");
            Assert.NotNull(result);
        }

        [Fact]
        public void GetNewsFromApiNewsDataIoNotNull()
        {
            var config = new Config();
            config.Key = "apikey=" + "pub_2815763c25cffe45251bb8682ef275560ee69";
            config.Language = "&language=" + "vi";
            config.Item = "results";
            config.Url = "https://newsdata.io/api/1/news?" + config.Key + config.Language;
            IDataSource _dataSource = new ApiDataSource();

            var result = _dataSource.GetNews(config.Url);

            Assert.NotNull(result);
        }

        [Fact]
        public void ConvertDataFromTuoiTreNewsToArticlesNotNull()
        {
            IDataSource _dataSource = new RssDataSource();
            _config.Data = _dataSource.GetNews("https://tuoitre.vn/rss/tin-moi-nhat.rss");
            _config.Item = "item";
            _config.Channel = "Tuoi Tre News";

            var mappingConfig = _newsService.CreateMapping(_dataSample.GetRssTuoiTreNewsDataMappingConfiguration());
            var result = _dataSource.ConvertDataToArticles(_config, mappingConfig);

            Assert.NotNull(result);
        }

        [Fact]
        public void ConvertDataFromGgTrendsToArticlesNotNull()
        {
            IDataSource _dataSource = new RssDataSource();
            _config.Data = _dataSource.GetNews("https://trends.google.com.vn/trends/trendingsearches/daily/rss?geo=VN");
            _config.Item = "item";

            var mappingConfig = _newsService.CreateMapping(_dataSample.GetGgTrendsNewsDataMappingConfiguration());
            var result = _dataSource.ConvertDataToArticles(_config, mappingConfig);

            Assert.NotNull(result);
        }

        [Fact]
        public void ConvertDataFromNewsDataToArticlesNotNull()
        {
            IDataSource _dataSource = new ApiDataSource();
            _config.Key = "apikey=" + "pub_2815763c25cffe45251bb8682ef275560ee69";
            _config.Language = "&language=" + "vi";
            _config.Category = "&category=" + "business,entertainment";
            _config.Url = "https://newsdata.io/api/1/news?" + _config.Key + _config.Language + _config.Category;
            _config.Data = _dataSource.GetNews(_config.Url);
            _config.Item = "results";

            var mappingConfig = _newsService.CreateMapping(_dataSample.GetNewsDataIoMappingConfiguration());
            var result = _dataSource.ConvertDataToArticles(_config, mappingConfig);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetWeatherInforNotNull()
        {
            var service = new NewsService();
            var config = new Config();
            config.Headers.RapidApiKey = "63e013be17mshfaa183691e3f9fap12264bjsn8690697c78c9";
            config.Headers.RapidApiHost = "weatherapi-com.p.rapidapi.com";
            config.KeyWork = "q=" + "Ho Chi Minh";
            config.DayNumber = "&day=" + "3";
            config.Language = "&lang=" + "vi";
            config.Location = "location";
            config.Url = "https://weatherapi-com.p.rapidapi.com/forecast.json?" + config.KeyWork + config.DayNumber + config.Language;

            var result = service.GetWeatherInfor(config);
            Assert.NotNull(result);
        }

        [Fact]
        public void ConvertDataToWeatherNotNull()
        {
            IDataSource dataSource = new ApiDataSource();
            var config = new Config();
            config.Headers.RapidApiKey = "63e013be17mshfaa183691e3f9fap12264bjsn8690697c78c9";
            config.Headers.RapidApiHost = "weatherapi-com.p.rapidapi.com";
            config.KeyWork = "q=" + "Ho Chi Minh";
            config.DayNumber = "&day=" + "3";
            config.Language = "&lang=" + "vi";
            config.Location = "location";
            config.Url = "https://weatherapi-com.p.rapidapi.com/forecast.json?" + config.KeyWork + config.DayNumber + config.Language;

            var weatherMappingConfig = _newsService.CreateMapping(_dataSample.GetWeatherMappingConfiguration());
            var data = dataSource.GetWeatherInfor(config);
            var result = dataSource.ConvertDataToWeather(data, weatherMappingConfig);

            Assert.NotNull(result);
            Assert.NotNull(result.HourlyWeather);
        }

        [Fact]
        public async Task AddArticleToDatabaseSuccess()
        {
            var data = new DbContext();
            var articleRepo = new ArticleRepository(data);
            var articleService = new ArticleService(articleRepo);
            Article article = _fixture.Create<Article>();

            var result = await articleService.Add(article);

            Assert.True(result);
        }
        
        [Theory]
        [InlineData("257b736d-8451-49b0-ac9d-20fbbf1e3e1b")]
        public async Task GetByArticleIdToDatabaseSuccess(string id)
        {
            var result = await _articleService.GetById(id);

            Assert.NotNull(result.Title);
            Assert.NotEmpty(result.Description);
        }

        [Theory]
        [InlineData("257b736d-8451-49b0-ac9d-20fbbf1e3e1b")]
        public async Task UpdateArticleToDatabaseSuccess(string id)
        {
            var newTitle = Guid.NewGuid().ToString();
            var article = await _articleService.GetById(id);

            article.Title = newTitle;
            var result = await _articleService.Update(article);
            var newArticle = await _articleService.GetById(article.Id.ToString());

            Assert.True(result);
            Assert.Equal(newArticle.Title, newTitle);
        }
    }
}