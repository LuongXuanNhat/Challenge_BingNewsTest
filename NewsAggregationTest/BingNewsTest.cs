using BingNew.DataAccessLayer.Entities;
using BingNew.DataAccessLayer.TestData;
using BingNew.Mapping;
using BingNew.Mapping.Interface;

namespace NewsAggregationTest
{
    public class BingNewsTest
    {
        private readonly Config _config;
        private readonly IApiDataSource _apiDataSource;
        private readonly IRssDataSource _rssDataSource;

        public BingNewsTest()
        {
            _config = new Config();
            _apiDataSource = new ApiDataSource();
            _rssDataSource = new RssDataSource();
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
        public void GetNewsFromRssTuoiTreNotNull()
        {
            Config config = new()
            {
                Url = "https://tuoitre.vn/rss/tin-moi-nhat.rss"
            };
            var result = _rssDataSource.GetData(config);
            Assert.NotNull(result);
        }

        [Fact]
        public void GetNewsFromRssGoogleTrendNotNull()
        {
            Config config = new()
            {
                Url = "https://trends.google.com.vn/trends/trendingsearches/daily/rss?geo=VN"
            };
            var result = _rssDataSource.GetData(config);
            Assert.NotNull(result);
        }

        [Fact]
        public void GetNewsFromApiNewsDataIoNotNull()
        {
            Config config = new()
            {
                Key = "apikey=" + DataSample.GetApiKeyOfNewsDataIo(),
                Language = "&language=" + "vi",
                Item = "results"
            };
            config.Url = "https://newsdata.io/api/1/news?" + config.Key + config.Language;

            var result = _apiDataSource.GetData(config);

            Assert.NotNull(result);
        }

        [Fact]
        public void ConvertDataFromTuoiTreNewsToArticlesNotNull()
        {
            Config config = new()
            {
                Url = "https://tuoitre.vn/rss/tin-moi-nhat.rss"
            };
            _config.Data = _rssDataSource.GetData(config);
            _config.Item = "item";
            _config.Channel = "Tuoi Tre News";

            var mappingConfig = DataSourceFactory.CreateMapping<List<CustomConfig>>(DataSample.GetRssTuoiTreNewsDataMappingConfiguration());

            var result = _rssDataSource.MultipleMapping(mappingConfig);

            Assert.NotNull(result);
        }

        [Fact]
        public void ConvertDataFromGgTrendsToArticlesNotNull()
        {
            Config config = new()
            {
                Url = "https://trends.google.com.vn/trends/trendingsearches/daily/rss?geo=VN"
            };
            _config.Data = _rssDataSource.GetData(config);
            _config.Item = "item";

            var mappingConfig = DataSourceFactory.CreateMapping<List<CustomConfig>>(DataSample.GetGgTrendsNewsDataMappingConfiguration());
            
            var result = _rssDataSource.MultipleMapping(mappingConfig);

            Assert.NotNull(result);
            Assert.True(result.Item1, result.Item3);
        }

        [Fact]
        public void ConvertDataFromNewsDataToArticlesNotNull()
        {
            _config.Key = "apikey=" + DataSample.GetApiKeyOfNewsDataIo();
            _config.Language = "&language=" + "vi";
            _config.Category = "&category=" + "business,entertainment";
            _config.Url = "https://newsdata.io/api/1/news?" + _config.Key + _config.Language + _config.Category;
            _config.Item = "results";

            var mappingConfig = DataSourceFactory.CreateMapping<List<CustomConfig>>(DataSample.GetNewsDataIoMappingConfiguration());
            var result = _apiDataSource.MultipleMapping(mappingConfig);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetWeatherInforNotNull()
        {
            var config = WeatherConfig();
            var result = _apiDataSource.GetData(config);
            Assert.NotNull(result);
        }

        // Single
        [Fact]
        public void ConvertDataToWeatherNotNull()
        {
            var weatherMappingConfig = DataSourceFactory.CreateMapping<List<CustomConfig>>(DataSample.GetWeatherConfiguration());

            var result = _apiDataSource.MultipleMapping(weatherMappingConfig);

            Assert.NotNull(result);
        }
        #endregion
    }
}