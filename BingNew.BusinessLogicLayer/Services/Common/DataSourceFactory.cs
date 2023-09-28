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
        public static CustomConfig CreateMapping(string jsonConfigMapping)
        {
            return JsonConvert.DeserializeObject<CustomConfig>(jsonConfigMapping) ?? new CustomConfig();
        }
        private static List<WeatherInfo> SetValueWeatherInfor(List<CustomConfig>? mappings, string? souPropertyPath, JObject? jsonObjects)
        {
            mappings = mappings ?? new List<CustomConfig>();
            var weatherInfos = new List<WeatherInfo>();
            var mapping = mappings.First(x => x.TableName.Equals(typeof(WeatherInfo).Name));
            jsonObjects ??= new JObject(); 
            
            var hourlyWeatherList = !string.IsNullOrEmpty(souPropertyPath) 
                ? jsonObjects.SelectToken(souPropertyPath) ?? new JArray() : new JArray();

            foreach (var item in hourlyWeatherList)
            {
                JObject jsonObject = JObject.Parse(item.ToString());
                weatherInfos.Add(ConvertDataToWeatherInfor(jsonObject, mapping));
            }
            return weatherInfos;
        }
        private static WeatherInfo ConvertDataToWeatherInfor(JObject jsonObject, CustomConfig weatherInfoMappingConfig)
        {
            var weatherInHour = new WeatherInfo();
            foreach (var obj in weatherInfoMappingConfig.MappingTables)
            {
                var sourceValue = jsonObject.SelectToken(obj.SouPropertyPath)?.ToString();
                obj.SouValue = sourceValue ?? string.Empty;

                var propertyInfo = typeof(WeatherInfo).GetProperty(obj.DesProperty);
                var getType = ParseDatatype(obj.DesDatatype);
                var convertedValue = GetValueHandler(getType, obj.SouValue);
                propertyInfo?.SetValue(weatherInHour, convertedValue);
            }
            return weatherInHour;
        }
    }
}
