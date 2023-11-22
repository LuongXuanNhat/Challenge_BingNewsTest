using Newtonsoft.Json.Linq;
using System.Collections;
using System.Xml.Linq;

namespace BingNew.Mapping.Interface
{
    public interface IChooseMapping
    {
       object MapData(CustomConfig customConfig, string data);
    }

    public class ListMapDataJson : IChooseMapping
    {
        public object MapData(CustomConfig customConfig, string data) 
        {
            Type objectType = MappingCommon.FindTypeByName(customConfig.TableName);
            Type listType = typeof(List<>).MakeGenericType(objectType);
            var articles = Activator.CreateInstance(listType) as IList ?? throw new InvalidOperationException("List is null");

            JObject jsonObject = JObject.Parse(data);
            var newsArray = jsonObject.SelectToken(customConfig.Config.Item) as JArray;
            foreach (JObject newsItem in newsArray?.OfType<JObject>() ?? Enumerable.Empty<JObject>())
            {
                var single = new SingleMapDataJson();
                var article = single.MapData(customConfig, newsItem.ToString());
                articles.Add(article);
            }

            return articles;
        }
    }
    public class ListMapDataXml : IChooseMapping
    {
        public object MapData(CustomConfig customConfig, string data) 
        {
            Type objectType = MappingCommon.FindTypeByName(customConfig.TableName);
            Type listType = typeof(List<>).MakeGenericType(objectType);
            var list = Activator.CreateInstance(listType) as IList ?? throw new InvalidOperationException("List is null");

            XDocument document = XDocument.Parse(data) ?? throw new InvalidOperationException("Could not get data");
            var newsArray = document.Descendants(customConfig.Config.Item);
            foreach (var item in newsArray)
            {
                SingleMapDataXml single = new();
                var article = single.MapData(customConfig, item.ToString());
                list.Add(article);
            }

            return list ;
        }
    }

    public class SingleMapDataJson : IChooseMapping
    {
        public object MapData(CustomConfig customConfig, string data)
        {
            JObject jsonObject = JObject.Parse(data);
            var objectType = MappingCommon.FindTypeByName(customConfig.TableName);
            dynamic obj = Activator.CreateInstance(objectType) ?? "";
            foreach (var config in customConfig.MappingTables)
            {
                config.SouValue = Convert.ToString(jsonObject.SelectToken(config.SouPropertyPath)) ?? "Undefined";
                var propertyInfo = objectType.GetProperty(config.DesProperty);
                var getType = DataSourceFactory.ParseDatatype(config.DesDatatype);
                var convertedValue = DataSourceFactory.GetValueByDataType(getType, config.SouValue, null, jsonObject, config.SouPropertyPath);
                propertyInfo?.SetValue(obj, convertedValue);
            }

            return obj;
        }
    }

    public class SingleMapDataXml : IChooseMapping
    {
        public object MapData(CustomConfig customConfig, string data)
        {
            var objectType = MappingCommon.FindTypeByName(customConfig.TableName);
            dynamic obj = Activator.CreateInstance(objectType) ?? "";
            var xml = XElement.Parse(data);
            foreach (var config in customConfig.MappingTables)
            {
                XNamespace ht = config.Namespace;
                config.SouValue = (config.Namespace != "")
                ? xml.Descendants(ht + config.SouPropertyPath).FirstOrDefault()?.Value ?? ""
                : GetSourceValue(xml, config) ?? string.Empty;

                var propertyInfo = objectType.GetProperty(config.DesProperty);
                var getType = DataSourceFactory.ParseDatatype(config.DesDatatype);
                var convertedValue = DataSourceFactory.GetValueByDataType(getType, config.SouValue, null, null, config.SouPropertyPath);
                propertyInfo?.SetValue(obj, convertedValue);
            }

            return obj;
        }
        private static string GetSourceValue(XElement item, MappingTable obj)
        {
            XNamespace ns = XNamespace.Get(obj.Namespace);
            var sourceElement = ns != null ? item.Element(ns + obj.SouPropertyPath) : item.Element(obj.SouPropertyPath);
            return sourceElement?.Value ?? string.Empty;
        }
    }

}