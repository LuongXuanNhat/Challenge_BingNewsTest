using BingNew.BusinessLogicLayer.Interfaces;
using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.BusinessLogicLayer.ModelConfig;
using BingNew.DataAccessLayer.Models;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BingNew.BusinessLogicLayer.Services.Common
{
    public class RssDataSource : IRssDataSource
    {


        public RssDataSource()
        {

        }

        private string DownloadXml(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                return client.GetStringAsync(url).Result;
            }
        }

        public string GetNews(string Url)
        {
            return DownloadXml(Url);
        }

        public List<Article> ConvertDataToArticles(Config config, List<MappingTable> mapping)
        {
            var articles = new List<Article>();
            XDocument document = XDocument.Parse(config.Data);
            var items = document.Descendants(config.Item);

            foreach (var item in items)
            {
                var article = MapToArticle(item, mapping);
                if(config.Channel != null)
                    article.SetChannel(config.Channel);
                articles.Add(article);
            }
            return articles;
        }

        private Article MapToArticle(XElement item, List<MappingTable> mapping)
        {
            var article = new Article();
            foreach (var obj in mapping)
            {
                XNamespace ns = XNamespace.Get(obj.Namespace);

                try
                {
                    var sourceValue = ns != null
                    ? item.Element(ns + obj.SouPropertyPath)?.Value
                    : item.Element(obj.SouPropertyPath)?.Value;

                    obj.SouValue = sourceValue ?? string.Empty;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                var propertyInfo = typeof(Article).GetProperty(obj.DesProperty);
                if (propertyInfo != null && propertyInfo.PropertyType != null)
                {
                    if (obj.SouDatatype == "string" && obj.DesDatatype == "DateTime")
                    {
                        string dateString = obj.SouValue;
                        dateString = dateString.Replace(" GMT+7", "");
                        CultureInfo culture = CultureInfo.InvariantCulture;
                        var convertedValue = DateTime.Parse(dateString, culture);
                        propertyInfo.SetValue(article, convertedValue);
                    }
                    else
                    {
                        var convertedValue = Convert.ChangeType(obj.SouValue, propertyInfo.PropertyType);
                        propertyInfo.SetValue(article, convertedValue);
                    }
                }
            }
            return article;
        }

        public string GetWeatherInfor(Config config)
        {
            throw new NotImplementedException();
        }

        public Weather ConvertDataToWeather(string data, List<MappingTable> mapping)
        {
            throw new NotImplementedException();
        }
    }
}