using BingNew.BusinessLogicLayer.Interfaces;
using BingNew.DataAccessLayer.Entities;
using BingNew.DataAccessLayer.Models;
using BingNew.Mapping;
using BingNew.Mapping.Interface;
using BingNew.ORM.DbContext;

namespace BingNew.BusinessLogicLayer.Services
{
    public class MappingService : IMappingService
    {

        private readonly IJsonDataSource _apiDataSource;
        private readonly IXmlDataSource _rssDataSource;
        private readonly DbBingNewsContext _dataContext;

        public MappingService(IJsonDataSource apiDataSource, IXmlDataSource rssDataSource, DbBingNewsContext dataContext)
        {
            _apiDataSource = apiDataSource;
            _rssDataSource = rssDataSource;
            _dataContext = dataContext;
        }

        public Tuple<bool, string> CrawlNewsJson(List<CustomConfig> customs)
        {
            try
            {
                var result = _apiDataSource.MapMultipleObjects(customs);
                var list = result.Item2.OfType<List<Article>>().ToList();
                _dataContext.AddRanger(list); 

                return new Tuple<bool, string>(true, "Crawl news data successfully!");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "Error: " + ex.Message);
            }
        }

        public Tuple<bool, string> CrawlNewsXml(List<CustomConfig> customs)
        {
            try
            {
                var result = _rssDataSource.MapMultipleObjects(customs);
                var list = result.Item2.OfType<List<Article>>().ToList();
                _dataContext.AddRanger(list);

                return new Tuple<bool, string>( true, "Crawl news data successfully!");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "Error: " + ex.Message);
            }
        }

        // Single
        public Tuple<bool, string> CrawlWeatherForecast(List<CustomConfig> customs)
        {
            try
            {
                var result = _apiDataSource.MapMultipleObjects(customs);
                var weather = result.Item2.OfType<Weather>().First() ?? throw new InvalidOperationException("no data is mapped");
                var weatherInfor = result.Item2.OfType<List<WeatherInfo>>().First() ?? throw new InvalidOperationException("no data is mapped in weatherInfo");

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
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "Error: " + ex.Message);
            }
        }
    }
}
