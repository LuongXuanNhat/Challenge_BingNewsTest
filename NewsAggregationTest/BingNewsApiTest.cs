﻿using BingNew.BusinessLogicLayer.Interfaces;
using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.BusinessLogicLayer.ModelConfig;
using BingNew.BusinessLogicLayer.Services;
using BingNew.BusinessLogicLayer.Services.Common;
using BingNew.DataAccessLayer.Entities;
using BingNew.DataAccessLayer.TestData;
using BingNew.DI;
using BingNew.ORM.DbContext;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace NewsAggregationTest
{
    public class BingNewsApiTest
    {
        private readonly Mock<IBingNewsService> _bingNewsService = new();
        private readonly DbBingNewsContext _dataContext = new();
        private readonly DIContainer _container = new();

        private readonly IBingNewsService _bingService;
        private readonly IApiDataSource _apiDataSource;
        private readonly IRssDataSource _rssDataSource;
        private readonly IMappingService _mappingService;

        public BingNewsApiTest()
        {
            _container.Register<DbBingNewsContext, DbBingNewsContext>();
            _container.Register<IApiDataSource, ApiDataSource>();
            _container.Register<IRssDataSource, RssDataSource>();
            _container.Register<IBingNewsService, BingNewsService>();
            _container.Register<IMappingService, MappingService>();

            _bingService = _container.Resolve<IBingNewsService>();
            _apiDataSource = _container.Resolve<IApiDataSource>();
            _rssDataSource = _container.Resolve<IRssDataSource>();
            _mappingService = _container.Resolve<IMappingService>();
        }

        [Fact]
        public void Get_All_Arrticle()
        {
            var result = _dataContext.GetAll<Article>();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
        [Fact]
        public void Get_Article_ByQuantity()
        {
            var result = _dataContext.GetAll<Article>().Take(5);
            Assert.Equal(5, result.ToList().Count);
        }
        [Fact]
        public void Get_All_Article_By_Quantity_By_Mock()
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
        public void Get_Trending_Articles_Panel()
        {
            var bingService = new BingNewsService(_dataContext);
            var articleTrend = bingService.GetTrendingArticlesPanel(12);
            Assert.NotEmpty(articleTrend);

        }
        [Fact]
        public void Get_Data_From_Api_Json_Success()
        {
            var dataMockup = DataSample.GetDataMockupNewsDataIo();
            var mappingCustom = DataSourceFactory.CreateMapping<List<CustomConfig>>(dataMockup);
            foreach (var item in mappingCustom)
            {
                item.Config.Data = _apiDataSource.GetNews(item.Config.Url);
                var result = _apiDataSource.ConvertDataToArticles<Article>(item.Config, mappingCustom);
                Assert.NotEmpty(result);
            }
        }
        [Fact]
        public void Get_Data_From_Api_Rss_Success()
        {
            var dataMockup = DataSample.GetDataMockupGgTrend();
            var mappingCustom = DataSourceFactory.CreateMapping<List<CustomConfig>>(dataMockup);
            foreach (var item in mappingCustom)
            {
                item.Config.Data = _rssDataSource.GetNews(item.Config.Url);
                var result = _rssDataSource.ConvertDataToArticles<Article>(item.Config, mappingCustom);
                Assert.NotEmpty(result);
            }
        }
        [Fact]
        public void Crawl_News_Json_Return_True()
        {
            var configData = DataSample.GetDataMockupNewsDataIo();
            var customConfigs = DataSourceFactory.CreateMapping<List<CustomConfig>>(configData);

            var result = _mappingService.CrawlNewsJson(customConfigs);

            Assert.True(result.Item1);
        }
        [Fact]
        public void Crawl_News_Xml_Return_True()
        {
            var configData = DataSample.GetDataMockupGgTrend();
            var customConfigs = DataSourceFactory.CreateMapping<List<CustomConfig>>(configData);

            var result = _mappingService.CrawlNewsXml(customConfigs);

            Assert.True(result.Item1);
        }
        [Fact]
        public void Get_Top_News()
        {
            var result = _bingService.GetTopNews(9);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void Crawl_Weather_Data_Successful()
        {
            var dataConfig = DataSample.GetWeatherConfiguration();
            var weatherMappingConfig = DataSourceFactory.CreateMapping<List<CustomConfig>>(dataConfig);
            try
            {
                Config config = weatherMappingConfig[0].Config;
                var data = _apiDataSource.GetWeatherInfor(config);
                var weatherVm = _apiDataSource.ConvertDataToType<WeatherVm>(data, weatherMappingConfig);
                
                Weather weather = new()
                {
                    Temperature = weatherVm.Temperature,
                    Description = weatherVm.Description,
                    Humidity = weatherVm.Humidity,
                    Icon = weatherVm.Icon,
                    Id = weatherVm.Id,
                    Place = weatherVm.Place,
                    PubDate = weatherVm.PubDate
                };
                foreach (var item in weatherVm.WeatherInfor)
                {
                    item.WeatherId = weather.Id;
                }
                _dataContext.Add(weather);
                _dataContext.AddRanger(weatherVm.WeatherInfor);

                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.False(true, ex.Message);
            }
        }

        [Fact]
        public void Get_Weather_Forecast_Successful()
        {
            var weather = _bingService.GetWeatherInDay(DateTime.Now);
            var weatherInfor = _bingService.GetWeatherInforInDay(DateTime.Now, weather.Id);

            WeatherVm result = new(weather, weatherInfor);
            Assert.NotNull(result);
            Assert.NotEmpty(result.WeatherInfor);
        }
    }
}