using BingNew.DataAccessLayer.Entities;
using Newtonsoft.Json.Linq;
using BingNew.BusinessLogicLayer.ModelConfig;
using System.Globalization;
using BingNew.BusinessLogicLayer.Interfaces.IService;
using System.Reflection;

namespace BingNew.BusinessLogicLayer.Services.Common
{
    public class ApiDataSource : IApiDataSource
    {
        public ApiDataSource()
        {
        }
        public string GetNews(string Url)
        {
            return DownloadJson(Url);
        }
        private static string DownloadJson(string Url)
        {
            using (HttpClient client = new HttpClient())
            {
                return client.GetStringAsync(Url).Result;
            }
        }
        public List<Article> ConvertDataToArticles(Config config, List<CustomConfig> mapping)
        {
            var articles = new List<Article>();
            JObject jsonObject = JObject.Parse(config.Data);

            if (jsonObject[config.Item] is JArray newsArray)
            {
                foreach (JObject newsItem in newsArray.OfType<JObject>())
                {
                    var article = MapToArticle(newsItem, mapping);
                    articles.Add(article);
                }
            }
            return articles;
        }
        private static Article MapToArticle(JObject item, List<CustomConfig> mapping)
        {
            var article = new Article();
            var articleMapping = mapping.First(x => x.TableName.Equals(typeof(Article).Name));
            mapping = mapping.Where(x => x != articleMapping).ToList();
            foreach (var obj in articleMapping.MappingTables)
            {
                var sourceValue = item[obj.SouPropertyPath]?.ToString();
                obj.SouValue = sourceValue ?? string.Empty;

                var propertyInfo = typeof(Article).GetProperty(obj.DesProperty);
                var getType = DataSourceFactory.ParseDatatype(obj.DesDatatype);
                var convertedValue = DataSourceFactory.GetValueHandler(getType, obj.SouValue, mapping);
                propertyInfo?.SetValue(article, convertedValue);

            }
            return article;
        }
        public string GetWeatherInfor(Config config)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(config.Url),
                Headers =
            {
                { "X-RapidAPI-Key", config.Headers.RapidApiKey },
                { "X-RapidAPI-Host", config.Headers.RapidApiHost },
            }
            };
            return GetDataWeather(request);
        }

        private static string GetDataWeather(HttpRequestMessage request)
        {
            var client = new HttpClient();
            using (var response = client.SendAsync(request).Result)
            {
                response.EnsureSuccessStatusCode();
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public Weather ConvertDataToWeather(string data, List<CustomConfig> mapping)
        {
            var weather = new Weather();
            JObject jsonObject = JObject.Parse(data);
            var weatherMapping = mapping.First(x => x.TableName.Equals(typeof(Weather).Name));
            mapping = mapping.Where(x => x != weatherMapping).ToList();
            foreach (var obj in weatherMapping.MappingTables)
            {
                var sourceValue = jsonObject.SelectToken(obj.SouPropertyPath)?.ToString();
                obj.SouValue = sourceValue ?? string.Empty;

                var propertyInfo = typeof(Weather).GetProperty(obj.DesProperty) ;
                var getType = DataSourceFactory.ParseDatatype(obj.DesDatatype);
                var convertedValue = DataSourceFactory.GetValueHandler(getType, obj.SouValue, mapping, jsonObject, obj.SouPropertyPath);
                propertyInfo?.SetValue(weather, convertedValue);
            }

            return weather;
        }
    }
}