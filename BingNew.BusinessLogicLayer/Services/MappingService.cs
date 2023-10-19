using BingNew.BusinessLogicLayer.Interfaces;
using BingNew.DataAccessLayer.Entities;
using BingNew.Mapping;
using BingNew.Mapping.Interface;
using BingNew.ORM.DbContext;

namespace BingNew.BusinessLogicLayer.Services
{
    public class MappingService : IMappingService
    {

        private readonly IJsonDataSource _jsonDataSource;
        private readonly IXmlDataSource _xmlDataSource;
        private readonly DbBingNewsContext _dataContext;

        public MappingService(IJsonDataSource apiDataSource, IXmlDataSource rssDataSource, DbBingNewsContext dataContext)
        {
            _jsonDataSource = apiDataSource;
            _xmlDataSource = rssDataSource;
            _dataContext = dataContext;
        }

        public Tuple<bool, string> CrawlNewsJson(List<CustomConfig> customs)
        {
            var result = _jsonDataSource.MapMultipleObjects(customs);
            foreach (var article in result)
            {
                _dataContext.AddRanger((List<Article>)article);
            }

            return new Tuple<bool, string>(true, "Crawl news data successfully!");
        }

        public Tuple<bool, string> CrawlNewsXml(List<CustomConfig> customs)
        {
            var result = _xmlDataSource.MapMultipleObjects(customs).ToList();
            foreach (var article in result)
            {
                _dataContext.AddRanger((List<Article>)article);
            }

            return new Tuple<bool, string>( true, "Crawl news data successfully!");
        }

        // Single
        public Tuple<bool, string> CrawlWeatherForecast(List<CustomConfig> customs)
        {
            var result = _jsonDataSource.MapMultipleObjects(customs);
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
            _dataContext.Add(weatherr);
            _dataContext.AddRanger(weatherInfor);

            return new Tuple<bool, string>(true, "Crawl weatehr forecast data successfully!");
        }
    }
}
