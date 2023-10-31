using BingNew.Mapping.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BingNew.Mapping
{
    // Factory Method Design Pattern
    public static class DataSourceFactory
    {
        public enum DataTypes
        {
            _string,
            _int,
            _float,
            _double,
            _DateTime,
            _DateTimeGMT,
            _DateTimeHour
        }

        private static readonly Dictionary<DataTypes, IDataTypeHandler> DataTypeHandlers = new()
        {
            { DataTypes._string, new StringHandler() },
            { DataTypes._int, new IntHandler() },
            { DataTypes._float, new FloatHandler() },
            { DataTypes._double, new DoubleHandler() },
            { DataTypes._DateTime, new DateTimeHandler() },
            ////{ DataTypes._DateTimeGMT, new DateTimeGMTHandler() },
            { DataTypes._DateTimeHour, new DateTimeHourHandler() },
        };

        public static DataTypes ParseDatatype(string input)
        {
            return Enum.TryParse(input, out DataTypes result)
                ? result
                : throw new ArgumentException("Invalue Datatypes: ", nameof(input));
        }
       
        public static T CreateMapFromJson<T>(string jsonConfigMapping) where T : new()
        {
            return JsonConvert.DeserializeObject<T>(jsonConfigMapping) ?? new T();
        }

        public static object GetValueByDataType(DataTypes dataType, string value, List<CustomConfig>? mapping = null, JObject? jsonObject = null, string? souPropertyPath = null)
        {
            DataTypeHandlers.TryGetValue(dataType, out var handler);
            return handler is not null ? handler.ConvertData(value, mapping, jsonObject, souPropertyPath)
                                        : throw new NotSupportedException("Datatype not supported");
        }
    }
}
