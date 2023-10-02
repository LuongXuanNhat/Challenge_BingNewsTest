﻿using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.BusinessLogicLayer.ModelConfig;
using BingNew.BusinessLogicLayer.Services.Common;
using BingNew.DataAccessLayer.Entities;
using BingNew.DataAccessLayer.TestData;
using Newtonsoft.Json;

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
            config.Key = "apikey=" + DataSample.GetApiKeyOfNewsDataIo();
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

            var mappingConfig = DataSourceFactory.CreateMapping(DataSample.GetRssTuoiTreNewsDataMappingConfiguration());
            var listConfigMapping = new List<CustomConfig>();
            listConfigMapping.Add(mappingConfig);
            var result = _rssDataSource.ConvertDataToArticles(_config, listConfigMapping);

            Assert.NotNull(result);
        }

        [Fact]
        public void ConvertDataFromGgTrendsToArticlesNotNull()
        {
            _config.Data = _rssDataSource.GetNews("https://trends.google.com.vn/trends/trendingsearches/daily/rss?geo=VN");
            _config.Item = "item";

            var mappingConfig = DataSourceFactory.CreateMapping(DataSample.GetGgTrendsNewsDataMappingConfiguration());
            var listConfigMapping = new List<CustomConfig>();
            listConfigMapping.Add(mappingConfig);
            var result = _rssDataSource.ConvertDataToArticles(_config, listConfigMapping);

            Assert.NotNull(result);
        }

        [Fact]
        public void ConvertDataFromNewsDataToArticlesNotNull()
        {
            _config.Key = "apikey=" + DataSample.GetApiKeyOfNewsDataIo();
            _config.Language = "&language=" + "vi";
            _config.Category = "&category=" + "business,entertainment";
            _config.Url = "https://newsdata.io/api/1/news?" + _config.Key + _config.Language + _config.Category;
            _config.Data = _apiDataSource.GetNews(_config.Url);
            _config.Item = "results";

            var mappingConfig = DataSourceFactory.CreateMapping(DataSample.GetNewsDataIoMappingConfiguration());
            var listConfigMapping = new List<CustomConfig>
            {
                mappingConfig
            };
            var result = _apiDataSource.ConvertDataToArticles(_config, listConfigMapping);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetWeatherInforNotNull()
        {
            var config = WeatherConfig();
            var result = _apiDataSource.GetWeatherInfor(config);
            Assert.NotNull(result);
        }

        [Fact]
        public void ConvertDataToWeatherNotNull()
        {
            var config = WeatherConfig();
            var listConfigMapping = new List<CustomConfig>();
            var weatherMappingConfig = DataSourceFactory.CreateMapping(DataSample.GetWeatherMappingConfiguration());
            var weatherInforMappingConfig = DataSourceFactory.CreateMapping(DataSample.GetWeatherInforMappingConfiguration());
            listConfigMapping.Add(weatherMappingConfig);
            listConfigMapping.Add(weatherInforMappingConfig);
            var data = _apiDataSource.GetWeatherInfor(config);
            var result = _apiDataSource.ConvertDataToType<Weather>(data, listConfigMapping);

            Assert.NotNull(result);
        }
        #endregion
    }
}