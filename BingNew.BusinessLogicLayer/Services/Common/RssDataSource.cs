using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.BusinessLogicLayer.ModelConfig;
using BingNew.DataAccessLayer.Entities;
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
            XDocument document = XDocument.Parse(config.Data);
            var items = document.Descendants(config.Item);

            foreach (var item in items)
            {
                var article = MapToArticle(item, mapping);
                articles.Add(article);
            }
            return articles;
        }

        private static Article MapToArticle(XElement item, List<CustomConfig> mapping)
        {
            var article = new Article();
            var articleMapping = mapping.First(x => x.TableName.Equals(typeof(Article).Name));
            mapping = mapping.Where(x => x != articleMapping).ToList();
            foreach (var obj in articleMapping.MappingTables)
            {
                obj.SouValue = GetSourceValue(item, obj);
                var propertyInfo = typeof(Article).GetProperty(obj.DesProperty);
                var getType = DataSourceFactory.ParseDatatype(obj.DesDatatype);
                var convertedValue = DataSourceFactory.GetValueHandler(getType, obj.SouValue, mapping);
                propertyInfo?.SetValue(article, convertedValue);
            }
            return article;
        }
        private static string GetSourceValue(XElement item, MappingTable obj)
        {
            XNamespace ns = XNamespace.Get(obj.Namespace);
            var sourceElement = (ns != null) ? item.Element(ns + obj.SouPropertyPath) : item.Element(obj.SouPropertyPath);
            var sourceValue = sourceElement?.Value;
            return sourceValue ?? string.Empty;
        }
        public string GetWeatherInfor(Config config)
        {
            throw new NotImplementedException();
        }

        public Weather ConvertDataToWeather(string data, List<CustomConfig> mapping)
        {
            throw new NotImplementedException();
        }
    }
}