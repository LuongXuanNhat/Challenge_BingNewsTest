using BingNew.BusinessLogicLayer.Interfaces;
using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.BusinessLogicLayer.Services;
using BingNew.DataAccessLayer.Entities;
using BingNew.DataAccessLayer.TestData;
using BingNew.DI;
using BingNew.Mapping;
using BingNew.Mapping.Interface;
using BingNew.ORM.DbContext;

namespace NewsAggregationTest
{
    public class BingNewsTest
    {
        private readonly Config _config;
        private readonly DIContainer _container = new();
        private readonly IJsonDataSource _apiDataSource;
        private readonly IXmlDataSource _rssDataSource;
        private readonly IMappingService _mappingService;
        private readonly IBingNewsService _bingServece;
        public BingNewsTest()
        {
            _config = new Config();
            _apiDataSource = new JsonDataSource();
            _rssDataSource = new XmlDataSource();

            _container.Register<DbBingNewsContext, DbBingNewsContext>();
            _container.Register<IBingNewsService, BingNewsService>();
            _container.Register<IXmlDataSource, XmlDataSource>();

            _container.Register<IMappingService, MappingService>();
            _mappingService = _container.Resolve<IMappingService>();
            _bingServece = _container.Resolve<IBingNewsService>();
        }

        private static Config WeatherConfig()
        {
            var config = new Config();
            config.Headers.RapidApiKey = DataSample.GetApiKeyOfWeather();
            config.Headers.RapidApiHost = "weatherapi-com.p.rapidapi.com";
            config.KeyWork = "q=" + "Ho Chi Minh";
            config.DayNumber = "&day=" + "3";
            config.Language = "&lang=" + "vi";
            config.Location = "location";
            config.Url = "https://weatherapi-com.p.rapidapi.com/forecast.json?" + config.KeyWork + config.DayNumber + config.Language;
            return config;
        }

        #region BingNews

        [Fact]
        public void Get_News_From_Rss_TuoiTre_Not_Null()
        {
            Config config = new()
            {
                Url = "https://tuoitre.vn/rss/tin-moi-nhat.rss"
            };
            var result = _rssDataSource.FetchData(config);
            Assert.NotNull(result);
        }

        [Fact]
        public void Get_News_From_Rss_Google_Trend_Not_Null()
        {
            Config config = new()
            {
                Url = "https://trends.google.com.vn/trends/trendingsearches/daily/rss?geo=VN"
            };
            var result = _rssDataSource.FetchData(config);
            Assert.NotNull(result);
        }

        [Fact]
        public void Get_News_From_Api_News_DataIo_Not_Null()
        {
            Config config = new()
            {
                Key = "apikey=" + DataSample.GetApiKeyOfNewsDataIo(),
                Language = "&language=" + "vi",
                Item = "results"
            };
            config.Url = "https://newsdata.io/api/1/news?" + config.Key + config.Language;

            var result = _apiDataSource.FetchData(config);

            Assert.NotNull(result);
        }

        [Fact]
        public void Convert_Data_From_TuoiTreNews_To_Articles_Not_Null()
        {
            Config config = new()
            {
                Url = "https://tuoitre.vn/rss/tin-moi-nhat.rss"
            };
            _config.Data = _rssDataSource.FetchData(config);
            _config.Item = "item";
            _config.Channel = "Tuoi Tre News";

            var mappingConfig = DataSourceFactory.CreateMapFromJson<List<CustomConfig>>(DataSample.GetRssTuoiTreNewsDataMappingConfiguration());

            var result = _rssDataSource.MapMultipleObjects(mappingConfig);

            Assert.NotNull(result);
        }

        [Fact]
        public void Convert_Data_From_GgTrends_To_Articles_Not_Null()
        {
            Config config = new()
            {
                Url = "https://trends.google.com.vn/trends/trendingsearches/daily/rss?geo=VN"
            };
            _config.Data = _rssDataSource.FetchData(config);
            _config.Item = "item";

            var mappingConfig = DataSourceFactory.CreateMapFromJson<List<CustomConfig>>(DataSample.GetGgTrendsNewsDataMappingConfiguration());
            
            var result = _rssDataSource.MapMultipleObjects(mappingConfig);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void Convert_Data_From_NewsData_To_Articles_Not_Null()
        {
            _config.Key = "apikey=" + DataSample.GetApiKeyOfNewsDataIo();
            _config.Language = "&language=" + "vi";
            _config.Category = "&category=" + "business,entertainment";
            _config.Url = "https://newsdata.io/api/1/news?" + _config.Key + _config.Language + _config.Category;
            _config.Item = "results";

            var mappingConfig = DataSourceFactory.CreateMapFromJson<List<CustomConfig>>(DataSample.GetNewsDataIoMappingConfiguration());
            var result = _apiDataSource.MapMultipleObjects(mappingConfig);

            Assert.NotNull(result);
        }

        [Fact]
        public void Crawl_Data_From_Google_News() 
        {
            var configData = DataSample.GetDataMockupGgNew();
            var customConfigs = DataSourceFactory.CreateMapFromJson<List<CustomConfig>>(configData);

            var result = _mappingService.CrawlNewsXml(customConfigs);

            Assert.True(result);
        }

        [Fact]
        public void Get_Weather_Infor_Not_Null()
        {
            var config = WeatherConfig();
            var result = _apiDataSource.FetchData(config);
            Assert.NotNull(result);
        }

        // Single
        [Fact]
        public void Convert_Data_To_Weather_Not_Null()
        {
            var weatherMappingConfig = DataSourceFactory.CreateMapFromJson<List<CustomConfig>>(DataSample.GetWeatherConfiguration());

            var result = _apiDataSource.MapMultipleObjects(weatherMappingConfig);

            Assert.NotNull(result);
        }
        #endregion

        #region Recommendation 

        [Fact]
        public void Simple_Recommendation()
        {
            var getTredingNews = _bingServece.GetTopNews(50);
            Assert.NotNull(getTredingNews);
        }

        [Fact]
        public void Add_User_Success()
        {
            Users users = new()
            {
                Id = Guid.NewGuid(),
                Email = string.Concat(Guid.NewGuid().ToString("N").AsSpan()[..8], "@gmail.com"),
                UserName = string.Concat("User ", DateTime.Now.Millisecond.ToString())
            };
            var result = _bingServece.RegisterUser(users);
            Assert.True(result);
        }

        // User Id 7a0443d6-0704-4524-8218-178e705228ba
        [Fact]
        public void Add_Interactive_Data1()
        {
            var articles = _bingServece.GetTrendingArticlesPanel(100, 10);
            var article = new Random().Next(0, articles.Count);
            var userInteraction = new UserInteraction()
            {
                ArticleId = articles[article].Id,
                UserId = Guid.Parse("7a0443d6-0704-4524-8218-178e705228ba"),
                Likes = 1
            };
            var result = _bingServece.AddUserInteraction(userInteraction);
            Assert.True(result);

        }

        [Fact]
        public void Add_Interactive_Data2()
        {
            var articles = _bingServece.GetTrendingArticlesPanel(100, 10);
            var article = new Random().Next(0, articles.Count);
            var userInteraction = new UserInteraction()
            {
                ArticleId = articles[article].Id,
                UserId = Guid.Parse("7a0443d6-0704-4524-8218-178e705228ba"),
                Dislike = 1
            };
            var result = _bingServece.AddUserInteraction(userInteraction);
            Assert.True(result);
        }

        [Fact] 
        public void Form_Like_To_DisLike()
        {
            var userInteraction = new UserInteraction()
            {
                ArticleId = Guid.Parse("4A1D5DC5-C562-4F4E-840F-8D18BF150006"),
                UserId = Guid.Parse("7a0443d6-0704-4524-8218-178e705228ba"),
                Dislike = 1
            };
            var result = _bingServece.AddUserInteraction(userInteraction);
            Assert.True(result);
        }

        [Fact]
        public void Form_DisLike_To_Like()
        {
            var userInteraction = new UserInteraction()
            {
                ArticleId = Guid.Parse("4A1D5DC5-C562-4F4E-840F-8D18BF150006"),
                UserId = Guid.Parse("7a0443d6-0704-4524-8218-178e705228ba"),
                Likes = 1
            };
            var result = _bingServece.AddUserInteraction(userInteraction);
            Assert.True(result);
        }

        [Fact]
        public void Remove_Like()
        {
            var userInteraction = new UserInteraction()
            {   
                ArticleId = Guid.Parse("4A1D5DC5-C562-4F4E-840F-8D18BF150006"),
                UserId = Guid.Parse("7a0443d6-0704-4524-8218-178e705228ba"),
                Likes = 1
            };
            userInteraction.SetId(100);
            var result = _bingServece.DeleteUserInteraction(userInteraction);
            Assert.True(result);
        }
        [Fact]
        public void Add_User_CLick_Data()
        {
            var articles = _bingServece.GetTrendingArticlesPanel(1, 2);
            var userId = Guid.Parse("557b6016-e833-4ebb-8fb8-c1e7fa2f0543");
            foreach (var item in articles)
            {
                var userClick = new UserClickEvent()
                {
                    ArticleId = item.Id,
                    UserId = userId,
                    Id = Guid.NewGuid()
                };

                var result = _bingServece.AddUserClick(userClick);
                Assert.True(result);
            }
        }
        [Fact]
        public async Task Get_Number_Click_Article_Of_User()
        {
            var userId = Guid.Parse("a17e20c0-c84a-447b-a468-9253cc2cfe4c");
            var result = await _bingServece.Recommendation(userId);
            Assert.NotEmpty(result);
        }

        #endregion

        
    }
}