﻿using Newtonsoft.Json.Linq;
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
            return DateTime.Parse(dateTimeValue, culture);
        }
    }
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
