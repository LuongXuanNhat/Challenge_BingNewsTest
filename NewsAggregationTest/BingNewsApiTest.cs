using BingNew.BusinessLogicLayer.Interfaces;
using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.BusinessLogicLayer.Services;
using BingNew.DataAccessLayer.Entities;
using BingNew.DataAccessLayer.TestData;
using BingNew.DI;
using BingNew.Mapping;
using BingNew.Mapping.Interface;
using BingNew.ORM.DbContext;
using System.Diagnostics;
using Xunit.Abstractions;

namespace NewsAggregationTest
{
    public class BingNewsApiTest
    {
        private readonly DbBingNewsContext _dataContext = new();
        private readonly DIContainer _container = new();

        private readonly IBingNewsService _bingService;
        private readonly IJsonDataSource _apiDataSource;
        private readonly IXmlDataSource _rssDataSource;
        private readonly IMappingService _mappingService;
        private readonly ITestOutputHelper _output;

        public BingNewsApiTest(ITestOutputHelper output)
        {
            _output = output;
            _container.Register<DbBingNewsContext, DbBingNewsContext>();
            _container.Register<IJsonDataSource, JsonDataSource>();
            _container.Register<IXmlDataSource, XmlDataSource>();
            _container.Register<IBingNewsService, BingNewsService>();
            _container.Register<IMappingService, MappingService>();

            _bingService = _container.Resolve<IBingNewsService>();
            _apiDataSource = _container.Resolve<IJsonDataSource>();
            _rssDataSource = _container.Resolve<IXmlDataSource>();
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
        public void Get_Trending_Articles_Panel()
        {
            var articleTrend = _bingService.GetTrendingArticlesPanel(20);
            Assert.NotEmpty(articleTrend);
        }
        [Fact]
        public void Get_Data_From_Api_Json_Success()
        {
            var dataMockup = DataSample.GetDataMockupNewsDataIo();
            var mappingCustom = DataSourceFactory.CreateMapFromJson<List<CustomConfig>>(dataMockup);
            
            var result = _apiDataSource.MapMultipleObjects(mappingCustom);
            Assert.NotNull(result);
        }
        [Fact]
        public void Get_Data_From_Api_Rss_Success()
        {
            var dataMockup = DataSample.GetDataMockupGgTrend();
            var mappingCustom = DataSourceFactory.CreateMapFromJson<List<CustomConfig>>(dataMockup);

            var result = _rssDataSource.MapMultipleObjects(mappingCustom);
            Assert.NotNull(result);
        }
        [Fact] 
        public void Crawl_News_Json_Return_True()
        {
            var configData = DataSample.GetDataMockupNewsDataIo();
            var customConfigs = DataSourceFactory.CreateMapFromJson<List<CustomConfig>>(configData);

            var result = _mappingService.CrawlNewsJson(customConfigs);

            Assert.True(result); 
        }
        [Fact] 
        public void Crawl_News_Json_Return_True_Using_Parallel()
        {
            var configData = DataSample.GetDataMockupNewsDataIo();
            var customConfigs = DataSourceFactory.CreateMapFromJson<List<CustomConfig>>(configData);

            var result = _mappingService.CrawlNewsJsonByParallel(customConfigs);

            Assert.True(result); 
        }
        [Fact]
        public void Crawl_News_Xml_Return_True()
        {
            var configData = DataSample.GetDataMockupGgTrend();
            var customConfigs = DataSourceFactory.CreateMapFromJson<List<CustomConfig>>(configData);

            var result = _mappingService.CrawlNewsXml(customConfigs);

            Assert.True(result);
        }
       
        [Fact]
        public void Get_Top_News_Successful()
        {
            var result = _bingService.GetTopNews(9);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        // Single
        [Fact]
        public void Crawl_Weather_Data_Successful()
        {
            var dataConfig = DataSample.GetWeatherConfiguration();
            var weatherMappingConfig = DataSourceFactory.CreateMapFromJson<List<CustomConfig>>(dataConfig);

            var result = _apiDataSource.MapMultipleObjects(weatherMappingConfig);
            var weather = result.OfType<Weather>().First() ?? throw new InvalidOperationException("no data is mapped");
            var weatherInfor = result.OfType<List<WeatherInfo>>().First() ?? throw new InvalidOperationException("no data is mapped in weatherInfo");

            Weather weatherr = new()
            {
                Temperature = weather.Temperature,
                Description = weather.Description,
                Humidity = weather.Humidity,
                Icon = weather.Icon,
                Id = weather.Id,
                Place = weather.Place,
                PubDate = weather.PubDate
            };
            foreach (var item in weatherInfor)
            {
                item.WeatherId = weather.Id;
            }
            _bingService.AddWeather(weatherr);
            _bingService.AddWeatherRanger(weatherInfor);


            Assert.True(true);
        }

        [Fact]
        public async Task Get_Weather_Forecast_Successful()
        {
            var weather = await _bingService.GetWeatherInDay(DateTime.Now);
            var weatherInfor = await _bingService.GetWeatherInforInDay(DateTime.Now, weather.Id);

            WeatherVm result = new(weather, weatherInfor);
            Assert.NotNull(result);
            Assert.NotEmpty(result.WeatherInfor);
        }
        [Fact]
        public void Test_Crawl_Mapping_Return_True()
        {
            var dataConfig = DataSample.GetWeatherConfiguration();
            var weatherMappingConfig = DataSourceFactory.CreateMapFromJson<List<CustomConfig>>(dataConfig);
            var result = _mappingService.CrawlWeatherForecast(weatherMappingConfig);
            Assert.True(result);
        }

        [Fact] 
        public void Get_Weather_Forecast_Api_Successful()
        {
            var result = _bingService.GetWeatherForecast(DateTime.Now);
            Assert.NotNull(result);
        }

        [Fact]
        public void Search_Data_Successful()
        {
            var result = _bingService.Search("chiến thắng");
            Assert.NotNull(result);
        }

        [Fact]
        public void Search_Data_Successful2()
        {
            var result = _bingService.Search("nông sản việt");
            Assert.NotNull(result);
        }


        [Fact]
        public void Search_Data_Successful3()
        {
            var result = _bingService.Search("vụ thảm sát đẫm máu");
            Assert.NotNull(result);
        }

        [Fact]
        public void Test_Full_Text_Search()
        {
            var task = Task.Run(async () => await Full_Text_Search());

            task.Wait();

            Assert.True(task.IsCompleted);
        }
        private async Task Full_Text_Search()
        {
            var result = await _bingService.FullTextSearch("vụ thảm sát");
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Add_Advertisement()
        {
            var ad = new AdArticle()
            {
                Id = Guid.NewGuid(),
                Description = "description",
                Link = "link",
                MediaLink = "link",
                PubDate = DateTime.Now,
                Title = "title"
            };
            var result = await _bingService.AddAdvertisement(ad);
            Assert.True(result);
        }

        [Fact]
        public void Get_Advertisement()
        {
            var result = _dataContext.GetAll<AdArticle>();
            Assert.NotNull(result);
        }

        #region ... Parallel 
        [Fact]
        public async Task Using_Foreach_AddRanger_Advertisement()
        {
            List<AdArticle> ads = new()
            {
                new AdArticle()
                {
                    Id = Guid.NewGuid(),
                    Description = "description",
                    Link = "link",
                    MediaLink = "link",
                    PubDate = DateTime.Now,
                    Title = "title"
                },
                new AdArticle()
                {
                    Id = Guid.NewGuid(),
                    Description = "description",
                    Link = "link",
                    MediaLink = "link",
                    PubDate = DateTime.Now,
                    Title = "title"
                },
                new AdArticle()
                {
                    Id = Guid.NewGuid(),
                    Description = "description",
                    Link = "link",
                    MediaLink = "link",
                    PubDate = DateTime.Now,
                    Title = "title"
                },
                new AdArticle()
                {
                    Id = Guid.NewGuid(),
                    Description = "description",
                    Link = "link",
                    MediaLink = "link",
                    PubDate = DateTime.Now,
                    Title = "title"
                },
                new AdArticle()
                {
                    Id = Guid.NewGuid(),
                    Description = "description",
                    Link = "link",
                    MediaLink = "link",
                    PubDate = DateTime.Now,
                    Title = "title"
                }
                ,new AdArticle()
                {
                    Id = Guid.NewGuid(),
                    Description = "description",
                    Link = "link",
                    MediaLink = "link",
                    PubDate = DateTime.Now,
                    Title = "title"
                },
                new AdArticle()
                {
                    Id = Guid.NewGuid(),
                    Description = "description",
                    Link = "link",
                    MediaLink = "link",
                    PubDate = DateTime.Now,
                    Title = "title"
                },
                new AdArticle()
                {
                    Id = Guid.NewGuid(),
                    Description = "description",
                    Link = "link",
                    MediaLink = "link",
                    PubDate = DateTime.Now,
                    Title = "title"
                },

            };
            Stopwatch stopwatch = new();
            stopwatch.Start();
                var service = await _bingService.AddRangerAdver(ads);
            stopwatch.Stop();
            _output.WriteLine("Thời gian chạy là: " + stopwatch.ElapsedTicks.ToString());
            Assert.True(service);
        }
        
        
        #endregion
    }
}
