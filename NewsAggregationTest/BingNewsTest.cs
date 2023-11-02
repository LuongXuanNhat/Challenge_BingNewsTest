using BingNew.BusinessLogicLayer.Interfaces;
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
        public BingNewsTest()
        {
            _config = new Config();
            _apiDataSource = new JsonDataSource();
            _rssDataSource = new XmlDataSource();

            _container.Register<IXmlDataSource, XmlDataSource>();
            _container.Register<DbBingNewsContext, DbBingNewsContext>();

            _container.Register<IMappingService, MappingService>();
            _mappingService = _container.Resolve<IMappingService>();
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
    }
}