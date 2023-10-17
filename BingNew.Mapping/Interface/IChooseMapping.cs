using BingNew.Mapping;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BingNew.Mapping.Interface
{
    public interface IChooseMapping
    {
       object HandleData(CustomConfig customConfig, string data);
    }

    public class ListMapData : IChooseMapping
    {
        public object HandleData(CustomConfig customConfig, string data) 
        {
            Type objectType = MappingCommon.FindTypeByName(customConfig.TableName);
            Type listType = typeof(List<>).MakeGenericType(objectType);
            var articles = Activator.CreateInstance(listType) as IList ?? throw new InvalidOperationException("List is null");

            JObject jsonObject = data != null ? JObject.Parse(data)
                : throw new InvalidOperationException("Could not get data");
            var newsArray = jsonObject.SelectToken(customConfig.Config.Item) as JArray;
          ////  newsArray = customConfig.SouPath is not null ? newsArray[customConfig.SouPath] as JArray : newsArray;
            foreach (JObject newsItem in newsArray?.OfType<JObject>() ?? Enumerable.Empty<JObject>())
            {
                var single = new SingleMapData();
                var article = single.HandleData(customConfig, newsItem.ToString());
                articles.Add(article);
            }

            return articles;
        }
    }
    public class ListMapDataXml : IChooseMapping
    {
        public object HandleData(CustomConfig customConfig, string data) 
        {
            Type objectType = MappingCommon.FindTypeByName(customConfig.TableName);
            Type listType = typeof(List<>).MakeGenericType(objectType);
            var list = Activator.CreateInstance(listType) as IList ?? throw new InvalidOperationException("List is null");

            XDocument document = data != null ? XDocument.Parse(data)
                : throw new InvalidOperationException("Could not get data");
            var newsArray = document.Descendants(customConfig.Config.Item);
            foreach (var item in newsArray)
            {
                SingleMapDataXml single = new();
                var article = single.HandleData(customConfig, item.ToString());
                list.Add(article);
            }

            return list ;
        }
    }

    public class SingleMapData : IChooseMapping
    {
        public object HandleData(CustomConfig customConfig, string data)
        {
            JObject jsonObject = JObject.Parse(data);
            var objectType = MappingCommon.FindTypeByName(customConfig.TableName);
            dynamic obj = Activator.CreateInstance(objectType) ?? "";
            foreach (var config in customConfig.MappingTables)
            {
                config.SouValue = Convert.ToString(jsonObject.SelectToken(config.SouPropertyPath)) ?? string.Empty;

                var propertyInfo = objectType.GetProperty(config.DesProperty);
                var getType = DataSourceFactory.ParseDatatype(config.DesDatatype);
                var convertedValue = DataSourceFactory.GetValueHandler2(getType, config.SouValue, null, jsonObject, config.SouPropertyPath);
                propertyInfo?.SetValue(obj, convertedValue);
            }

            return obj;
        }
    }

    public class SingleMapDataXml : IChooseMapping
    {
        public object HandleData(CustomConfig customConfig, string data)
        {
            var objectType = MappingCommon.FindTypeByName(customConfig.TableName);
            dynamic obj = Activator.CreateInstance(objectType) ?? "";

            foreach (var config in customConfig.MappingTables)
            {
                config.SouValue = GetSourceValue(XElement.Parse(data), config) ?? string.Empty;

                var propertyInfo = objectType.GetProperty(config.DesProperty);
                var getType = DataSourceFactory.ParseDatatype(config.DesDatatype);
                var convertedValue = DataSourceFactory.GetValueHandler2(getType, config.SouValue, null, null, config.SouPropertyPath);
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