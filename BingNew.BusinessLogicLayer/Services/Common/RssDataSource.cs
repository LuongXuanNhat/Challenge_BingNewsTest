using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.BusinessLogicLayer.ModelConfig;
using BingNew.DataAccessLayer.Entities;
using System.Data.SqlTypes;
using System.Xml.Linq;

namespace BingNew.BusinessLogicLayer.Services.Common
{
    public class RssDataSource : IRssDataSource
    {


        public RssDataSource()
        {

        }

        public string GetNews(string Url)
        {
            using (HttpClient client = new HttpClient())
            {
                return client.GetStringAsync(Url).Result;
            }
        }

        public List<Article> ConvertDataToArticles(Config config, List<CustomConfig> mapping)
        {
            var articles = new List<Article>();
            XDocument document = (config.Data != null) ? XDocument.Parse(config.Data) 
                : throw new InvalidOperationException("Could not get data");
            var items = document.Descendants(config.Item);

            foreach (var item in items)
            {
                var article = ConvertDataToType<Article>(item.ToString(), mapping);
                articles.Add(article);
            }
            return articles;
        }
        public T ConvertDataToType<T>(string data, List<CustomConfig> mapping) where T : new()
        {
            var obj = new T();

            var articleMapping = mapping.First(x => x.TableName.Equals(typeof(Article).Name));
            mapping = mapping.Where(x => x != articleMapping).ToList();
            foreach (var config in articleMapping.MappingTables)
            {
                config.SouValue = GetSourceValue(XElement.Parse(data), config);
                var propertyInfo = typeof(Article).GetProperty(config.DesProperty);
                var getType = DataSourceFactory.ParseDatatype(config.DesDatatype);
                var convertedValue = DataSourceFactory.GetValueHandler(getType, config.SouValue, mapping);
                propertyInfo?.SetValue(obj, convertedValue);
            }
            return obj;
        }
        private static string GetSourceValue(XElement item, MappingTable obj)
        {
            XNamespace ns = XNamespace.Get(obj.Namespace);
            var sourceElement = (ns != null) ? item.Element(ns + obj.SouPropertyPath) : item.Element(obj.SouPropertyPath);
            return sourceElement?.Value ?? string.Empty;
        }
        public string GetWeatherInfor(Config config)
        {
            throw new NotImplementedException();
        }

        
    }
}