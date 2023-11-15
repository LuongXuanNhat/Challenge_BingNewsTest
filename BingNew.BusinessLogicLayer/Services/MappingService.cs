using BingNew.BusinessLogicLayer.Interfaces;
using BingNew.DataAccessLayer.Entities;
using BingNew.Mapping;
using BingNew.Mapping.Interface;
using BingNew.ORM.DbContext;
using BingNew.ORM.Query;
using System.Reflection;

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

        public bool CrawlNewsJson(List<CustomConfig> customs)
        {
            var result = _jsonDataSource.MapMultipleObjects(customs).ToList();
            var listObj = new List<string>();
            customs.ForEach(customs => { listObj.Add(customs.TableName); });

            for (int i = 0; i < result.Count; i++)
            {
                var type = SqlExtensionCommon.FindTypeByName(listObj[i]);
                MethodInfo addRangerMethod = typeof(DbBingNewsContext).GetMethod("AddRanger") ?? throw new InvalidOperationException("Add function is inactive");
                MethodInfo genericAddRanger = addRangerMethod.MakeGenericMethod(type);
                genericAddRanger.Invoke(_dataContext, new object[] { result[i] });
            }

            return true;
        }
        public bool CrawlNewsJsonByParallel(List<CustomConfig> customs)
        {
            var objs = _jsonDataSource.MapMultipleObjects(customs).ToList();
            var strings = customs.Select(custom => custom.TableName).ToList();

            Parallel.ForEach(Enumerable.Range(0, objs.Count), i =>
            {
                var typeObj = SqlExtensionCommon.FindTypeByName(strings[i]);

                MethodInfo addRangerMethod = typeof(DbBingNewsContext).GetMethod("AddRanger") ?? throw new InvalidOperationException("Add function is inactive");

                MethodInfo genericAddRanger = addRangerMethod.MakeGenericMethod(typeObj);

                genericAddRanger.Invoke(_dataContext, new object[] { objs[i] });
            });

            return true;
        }

        public bool CrawlNewsXml(List<CustomConfig> customs)
        {
            var result = _xmlDataSource.MapMultipleObjects(customs).ToList();
            List<string> listObj = new();
            customs.ForEach(customs => { listObj.Add(customs.TableName); });
            for (int index = 0; index < result.Count; index++)
            {
                Type type = SqlExtensionCommon.FindTypeByName(listObj[index]);
                MethodInfo addRanger = typeof(DbBingNewsContext).GetMethod("AddRanger") ?? throw new InvalidOperationException("Add Function is Inactive");
                MethodInfo genericAddRanger = addRanger.MakeGenericMethod(type);
                genericAddRanger.Invoke(_dataContext, new object[] { result[index] });
            }
            return true;
        }

        // Single
        public bool CrawlWeatherForecast(List<CustomConfig> customs)
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

            return true;
        }
    }
}
