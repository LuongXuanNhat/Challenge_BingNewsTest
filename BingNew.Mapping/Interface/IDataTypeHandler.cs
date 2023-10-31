using Newtonsoft.Json.Linq;
using System.Globalization;

namespace BingNew.Mapping.Interface
{
    public interface IDataTypeHandler
    {
        object ConvertData(string value, List<CustomConfig>? mapping = null, JObject? jsonObject = null, string? souPropertyPath = null);
    }
    public class StringHandler : IDataTypeHandler
    {
        public object ConvertData(string value, List<CustomConfig>? mapping = null, JObject? jsonObject = null, string? souPropertyPath = null)
        {
            return Convert.ChangeType(value, typeof(string));
        }
    }

    public class IntHandler : IDataTypeHandler
    {
        public object ConvertData(string value, List<CustomConfig>? mapping = null, JObject? jsonObject = null, string? souPropertyPath = null)
        {
            return Convert.ChangeType(value, typeof(int));
        }
    }

    public class FloatHandler : IDataTypeHandler
    {
        public object ConvertData(string value, List<CustomConfig>? mapping = null, JObject? jsonObject = null, string? souPropertyPath = null)
        {
            return Convert.ChangeType(value, typeof(float));
        }
    }

    public class DoubleHandler : IDataTypeHandler
    {
        public object ConvertData(string value, List<CustomConfig>? mapping = null, JObject? jsonObject = null, string? souPropertyPath = null)
        {
            return Convert.ChangeType(value, typeof(double));
        }
    }
    public class DateTimeHandler : IDataTypeHandler
    {
        public object ConvertData(string value, List<CustomConfig>? mapping = null, JObject? jsonObject = null, string? souPropertyPath = null)
        {
            var dateTimeValue = value.Replace(" GMT+7", "");
            CultureInfo culture = CultureInfo.InvariantCulture;
            return DateTime.Parse(dateTimeValue, culture).ToLocalTime();
        }
    }

    ////public class DateTimeGMTHandler : IDataTypeHandler
    ////{
    ////    public object ConvertData(string value, List<CustomConfig>? mapping = null, JObject? jsonObject = null, string? souPropertyPath = null)
    ////    {
    ////        DateTime utcDateTime = DateTime
    ////            .ParseExact(value, "ddd, dd MMM yyyy HH:mm:ss 'GMT'", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal);
            
    ////        TimeZoneInfo gmt7TimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
    ////        DateTime gmt7DateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, gmt7TimeZone);

    ////        return gmt7DateTime.ToLocalTime();
    ////    }
    ////}
    
    public class DateTimeHourHandler : IDataTypeHandler
    {
        public object ConvertData(string value, List<CustomConfig>? mapping = null, JObject? jsonObject = null, string? souPropertyPath = null)
        {
            var dateTimeValue = value.Replace(" GMT+7", "");
            CultureInfo culture = CultureInfo.InvariantCulture;
            return DateTime.Parse(dateTimeValue, culture).Hour;
        }
    }
}
