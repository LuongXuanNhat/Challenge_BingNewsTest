using System.Xml.Linq;
using BingNew.Mapping.Interface;

namespace BingNew.Mapping
{
    public class RssDataSource : IRssDataSource
    {


        public RssDataSource()
        {

        }

        public string GetNews(string Url)
        {
            using HttpClient client = new();
            return client.GetStringAsync(Url).Result;
        }

        public List<T> ConvertDataToArticles<T>(Config config, List<CustomConfig> mapping) where T : new()
        {
            var articles = new List<T>();
            XDocument document = config.Data != null ? XDocument.Parse(config.Data)
                : throw new InvalidOperationException("Could not get data");
            var items = document.Descendants(config.Item);

            foreach (var item in items)
            {
                var article = ConvertDataToType<T>(item.ToString(), mapping);
                articles.Add(article);
            }
            return articles;
        }
        public T ConvertDataToType<T>(string data, List<CustomConfig> mapping) where T : new()
        {
            var obj = new T();

            var articleMapping = mapping.First(x => x.TableName.Equals(typeof(T).Name));
            mapping = mapping.Where(x => x != articleMapping).ToList();
            foreach (var config in articleMapping.MappingTables)
            {
                config.SouValue = GetSourceValue(XElement.Parse(data), config);
                var propertyInfo = typeof(T).GetProperty(config.DesProperty);
                var getType = DataSourceFactory.ParseDatatype(config.DesDatatype);
                var convertedValue = DataSourceFactory.GetValueHandler(getType, config.SouValue, mapping);
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
        public string GetWeatherInfor(Config config)
        {
            throw new NotImplementedException();
        }


    }
}