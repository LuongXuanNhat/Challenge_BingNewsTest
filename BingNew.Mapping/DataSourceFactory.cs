using BingNew.DataAccessLayer.Entities;
using BingNew.Mapping.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace BingNew.Mapping
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

        private static readonly Dictionary<DataTypes, IDataTypeHandler> DataTypeHandlers = new()
        {
            { DataTypes._string, new StringHandler() },
            { DataTypes._int, new IntHandler() },
            { DataTypes._double, new DoubleHandler() },
            { DataTypes._DateTime, new DateTimeHandler() },
            { DataTypes._DateTimeHour, new DateTimeHourHandler() },
        };

        public static DataTypes ParseDatatype(string input)
        {
            return Enum.TryParse<DataTypes>(input, out DataTypes result)
                ? result
                : throw new ArgumentException("Invalue Datatypes", nameof(input));
        }
       
        public static T CreateMapping<T>(string jsonConfigMapping) where T : new()
        {
            return JsonConvert.DeserializeObject<T>(jsonConfigMapping) ?? new T();
        }

        public static object GetValueHandler(DataTypes dataType, string value, List<CustomConfig>? mapping = null, JObject? jsonObject = null, string? souPropertyPath = null)
        {
            DataTypeHandlers.TryGetValue(dataType, out var handler);
            return handler is not null ? handler.Handle(value, mapping, jsonObject, souPropertyPath)
                                        : throw new NotSupportedException("Datatype not supported");
        }
    }
}
