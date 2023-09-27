using AutoFixture;
using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.BusinessLogicLayer.ModelConfig;
using BingNew.BusinessLogicLayer.Services.Common;
using BingNew.DataAccessLayer.Constants;
using BingNew.DataAccessLayer.Models;
using BingNew.DataAccessLayer.TestData;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace NewsAggregationTest
{
    public class BingNewsTest
    {
        private readonly DataSample _dataSample;
        private readonly Config _config;
        private readonly IApiDataSource _apiDataSource;
        private readonly IRssDataSource _rssDataSource;

        public BingNewsTest()
        {
            _dataSample = new DataSample();
            _config = new Config();
            _apiDataSource = new ApiDataSource();
            _rssDataSource = new RssDataSource();
        }

        ////private Config WeatherConfig()
        ////{
        ////    var config = new Config();
        ////    config.Headers.RapidApiKey = _constantCommon.RapidApiKey;
        ////    config.Headers.RapidApiHost = "weatherapi-com.p.rapidapi.com";
        ////    config.KeyWork = "q=" + "Ho Chi Minh";
        ////    config.DayNumber = "&day=" + "3";
        ////    config.Language = "&lang=" + "vi";
        ////    config.Location = "location";
        ////    config.Url = "https://weatherapi-com.p.rapidapi.com/forecast.json?" + config.KeyWork + config.DayNumber + config.Language;
        ////    return config;
        ////}

        #region BingNews

        [Fact]
        public void GetNewsFromRssTuoiTreNotNull()
        {
            var result = _rssDataSource.GetNews("https://tuoitre.vn/rss/tin-moi-nhat.rss");
            Assert.NotNull(result);
        }

        [Fact]
        public void GetNewsFromRssGoogleTrendNotNull()
        {
            var result = _rssDataSource.GetNews("https://trends.google.com.vn/trends/trendingsearches/daily/rss?geo=VN");
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

            var result = _apiDataSource.GetNews(config.Url);

            Assert.NotNull(result);
        }

        [Fact]
        public void ConvertDataFromTuoiTreNewsToArticlesNotNull()
        {
            _config.Data = _rssDataSource.GetNews("https://tuoitre.vn/rss/tin-moi-nhat.rss");
            _config.Item = "item";
            _config.Channel = "Tuoi Tre News";

            var mappingConfig = DataSourceFactory.CreateMapping(_dataSample.GetRssTuoiTreNewsDataMappingConfiguration());
            var result = _rssDataSource.ConvertDataToArticles(_config, mappingConfig);

            Assert.NotNull(result);
        }

        [Fact]
        public void ConvertDataFromGgTrendsToArticlesNotNull()
        {
            _config.Data = _rssDataSource.GetNews("https://trends.google.com.vn/trends/trendingsearches/daily/rss?geo=VN");
            _config.Item = "item";

            var mappingConfig = DataSourceFactory.CreateMapping(_dataSample.GetGgTrendsNewsDataMappingConfiguration());
            var result = _rssDataSource.ConvertDataToArticles(_config, mappingConfig);

            Assert.NotNull(result);
        }

        [Fact]
        public void ConvertDataFromNewsDataToArticlesNotNull()
        {
            _config.Key = "apikey=" + "pub_2815763c25cffe45251bb8682ef275560ee69";
            _config.Language = "&language=" + "vi";
            _config.Category = "&category=" + "business,entertainment";
            _config.Url = "https://newsdata.io/api/1/news?" + _config.Key + _config.Language + _config.Category;
            _config.Data = _apiDataSource.GetNews(_config.Url);
            _config.Item = "results";

            var mappingConfig = DataSourceFactory.CreateMapping(_dataSample.GetNewsDataIoMappingConfiguration());
            var result = _apiDataSource.ConvertDataToArticles(_config, mappingConfig);

            Assert.NotNull(result);
        }

        ////[Fact]
        ////public void GetWeatherInforNotNull()
        ////{
        ////    var service = new NewsService();
        ////    var config = new Config();
        ////    config.Headers.RapidApiKey = _constantCommon.RapidApiKey;
        ////    config.Headers.RapidApiHost = "weatherapi-com.p.rapidapi.com";
        ////    config.KeyWork = "q=" + "Ho Chi Minh";
        ////    config.DayNumber = "&day=" + "3";
        ////    config.Language = "&lang=" + "vi";
        ////    config.Location = "location";
        ////    config.Url = "https://weatherapi-com.p.rapidapi.com/forecast.json?" + config.KeyWork + config.DayNumber + config.Language;

        ////    var result = service.GetWeatherInfor(config);
        ////    Assert.NotNull(result);
        ////}

        ////[Fact]
        ////public void ConvertDataToWeatherNotNull()
        ////{
        ////    var config = WeatherConfig();
        ////    var weatherMappingConfig = _newsService.CreateMapping(_dataSample.GetWeatherMappingConfiguration());
        ////    var data = _apiDataSource.GetWeatherInfor(config);
        ////    var result = _apiDataSource.ConvertDataToWeather(data, weatherMappingConfig);

        ////    Assert.NotNull(result);
        ////    Assert.NotNull(result.GetHourlyWeather());
        ////}
        #endregion

        #region DI & Api
        ////[Fact]
        ////public async Task AddArticleToDatabaseSuccess()
        ////{
        ////    Article article = _fixture.Create<Article>();

        ////    var result = await _articleService.Add(article);

        ////    Assert.True(result);
        ////}

        ////[Theory]
        ////[InlineData("257b736d-8451-49b0-ac9d-20fbbf1e3e1b")]
        ////public async Task GetByArticleIdToDatabaseSuccess(string id)
        ////{
        ////    var result = await _articleService.GetById(id);

        ////    Assert.NotNull(result.GetTitle());
        ////    Assert.NotEmpty(result.GetDescription());
        ////}

        ////[Theory]
        ////[InlineData("257b736d-8451-49b0-ac9d-20fbbf1e3e1b")]
        ////public async Task UpdateArticleToDatabaseSuccess(string id)
        ////{
        ////    var newTitle = Guid.NewGuid().ToString();
        ////    var article = await _articleService.GetById(id);

        ////    article.SetTitle(newTitle);
        ////    var result = await _articleService.Update(article);
        ////    var newArticle = await _articleService.GetById(article.GetId().ToString());

        ////    Assert.True(result);
        ////    Assert.Equal(newArticle.GetTitle(), newTitle);
        ////}

        ////[Fact]
        ////public async Task GetAllArticleNotNull()
        ////{
        ////    var articles = await _articleService.GetAll();
        ////    Assert.NotNull(articles);
        ////    Assert.NotEmpty(articles);
        ////}

        ////[Fact]
        ////public async Task DeleteArticleToDatabaseSuccess()
        ////{
        ////    Article article = _fixture.Create<Article>();
        ////    await _articleService.Add(article);

        ////    var result = await _articleService.Delete(article.GetId().ToString());

        ////    Assert.True(result);
        ////}

        ////[Fact]
        ////public async Task AddArticleToDatabaseFromTuoiTreNews()
        ////{
        ////    IDataSource _dataSource = new RssDataSource();
        ////    _config.Data = _dataSource.GetNews("https://tuoitre.vn/rss/tin-moi-nhat.rss");
        ////    _config.Item = "item";
        ////    _config.Channel = "Tuoi Tre News";
        ////    var mappingConfig = _newsService.CreateMapping(_dataSample.GetRssTuoiTreNewsDataMappingConfiguration());
        ////    var articles = _dataSource.ConvertDataToArticles(_config, mappingConfig);

        ////    var result = await _articleService.AddRange(articles);

        ////    Assert.True(result);
        ////}
        ////[Fact]
        ////public async Task AddArticleToDatabaseFromNewDataIo()
        ////{
        ////    _config.Key = "apikey=" + "pub_2815763c25cffe45251bb8682ef275560ee69";
        ////    _config.Language = "&language=" + "vi";
        ////    _config.Category = "&category=" + "business,entertainment";
        ////    _config.Url = "https://newsdata.io/api/1/news?" + _config.Key + _config.Language + _config.Category;
        ////    _config.Data = _apiDataSource.GetNews(_config.Url);
        ////    _config.Item = "results";

        ////    var mappingConfig = _newsService.CreateMapping(_dataSample.GetNewsDataIoMappingConfiguration());
        ////    var articles = _apiDataSource.ConvertDataToArticles(_config, mappingConfig);
        ////    var result = await _articleService.AddRange(articles);

        ////    Assert.True(result);
        ////}

        ////[Fact]
        ////public async Task GetTrendingNews()
        ////{
        ////    var result = await _articleService.TrendingStories();
        ////    Assert.NotNull(result);
        ////}

        ////[Fact]
        ////public void TestMappingArticle()
        ////{
        ////    var article = _fixture.Create<Article>();

        ////    var result = _mappingService.Map<Article, ArticleVm>(article);
        ////    Assert.NotNull(result);
        ////}

        ////[Fact]
        ////public async Task AddWeatherToDatabaseFromApi()
        ////{
        ////    var config = WeatherConfig();
        ////    var weatherMappingConfig = _newsService.CreateMapping(_dataSample.GetWeatherMappingConfiguration());
        ////    var data = _apiDataSource.GetWeatherInfor(config);
        ////    var wearther = _apiDataSource.ConvertDataToWeather(data, weatherMappingConfig);

        ////    var result = await _weatherService.Add(wearther);

        ////    Assert.True(result);
        ////}

        #endregion

    }

  
}