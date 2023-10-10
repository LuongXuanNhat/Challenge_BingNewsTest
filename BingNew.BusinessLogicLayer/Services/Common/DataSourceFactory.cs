using BingNew.BusinessLogicLayer.ModelConfig;
using BingNew.DataAccessLayer.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text.Json.Nodes;

namespace BingNew.BusinessLogicLayer.Services.Common
{
    // Factory Method Design Pattern
    public static class DataSourceFactory
    {
        public enum DataTypes
        {
            _string,
            _int,
            _DateTime,
            _double,
            _WeatherInfor,
            _DateTimeHour
        }
        public static DataTypes ParseDatatype(string input)
        {
            return Enum.TryParse<DataTypes>(input, out DataTypes result)
                ? result
                : throw new ArgumentException("Invalue Datatypes", nameof(input));
        }
        // Pattern Matching(C# version >= 7)
        public static object GetValueHandler(DataTypes dataType, string value, List<CustomConfig>? mapping = null, JObject? jsonObject = null, string? souPropertyPath = null)
        {
            return dataType switch
            {
                DataTypes._string => Convert.ChangeType(value, typeof(string)),
                DataTypes._int => Convert.ChangeType(value, typeof(int)),
                DataTypes._double => Convert.ChangeType(value, typeof(double)),
                DataTypes._DateTime => HandleDateTime(value),
                DataTypes._DateTimeHour => HandleDateTimeHour(value),
                DataTypes._WeatherInfor => SetValueWeatherInfor(mapping, souPropertyPath, jsonObject),
                _ => throw new NotSupportedException("Datatype not supported"),
            };
        }

        private static int HandleDateTimeHour(string value)
        {
            var dateTimeValue = value.Replace(" GMT+7", "");
            CultureInfo culture = CultureInfo.InvariantCulture;
            DateTime dateTime = DateTime.Parse(dateTimeValue, culture);
            return dateTime.Hour;
        }

        private static DateTime HandleDateTime(string value)
        {
            var dateTimeValue = value.Replace(" GMT+7", "");
            CultureInfo culture = CultureInfo.InvariantCulture;
            return DateTime.Parse(dateTimeValue, culture);
        }
        public static T CreateMapping<T>(string jsonConfigMapping) where T : new()
        {
            return JsonConvert.DeserializeObject<T>(jsonConfigMapping) ?? new T();
        }
        private static List<WeatherInfor> SetValueWeatherInfor(List<CustomConfig>? mappings, string? souPropertyPath , JObject? jsonObjects)
        {
            mappings ??= new List<CustomConfig>();
            jsonObjects ??= new JObject();
            souPropertyPath ??= "";
            var mapping = mappings.First(x => x.TableName.Equals(typeof(WeatherInfor).Name));
            var hourlyWeatherList = jsonObjects.SelectToken(souPropertyPath) as JArray ?? new JArray();

            return hourlyWeatherList
            .OfType<JObject>() 
            .Select(item => ConvertJsonToWeatherInfo(item, mapping))
            .ToList();
        }
        private static WeatherInfor ConvertJsonToWeatherInfo(JObject jsonObject, CustomConfig weatherInfoMappingConfig)
        {
            var weatherInHour = new WeatherInfor();
            foreach (var obj in weatherInfoMappingConfig.MappingTables)
            {
                obj.SouValue = Convert.ToString(jsonObject.SelectToken(obj.SouPropertyPath)) ?? string.Empty;

                var propertyInfo = typeof(WeatherInfor).GetProperty(obj.DesProperty);
                var getType = ParseDatatype(obj.DesDatatype);
                var convertedValue = GetValueHandler(getType, obj.SouValue);
                propertyInfo?.SetValue(weatherInHour, convertedValue);
            }
            return weatherInHour;
        }
    }
}
