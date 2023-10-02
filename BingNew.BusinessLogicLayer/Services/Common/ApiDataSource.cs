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
                    var article = ConvertDataToType<Article>(newsItem.ToString(), mapping);
                    articles.Add(article);
                }
            }
            return articles;
        }

        public T ConvertDataToType<T>(string data, List<CustomConfig> mapping) where T : new()
        {
            var obj = new T();
            JObject jsonObject = JObject.Parse(data);
            var mappingConfig = mapping.First(x => x.TableName.Equals(typeof(T).Name));
            mapping = mapping.Where(x => x != mappingConfig).ToList();
            foreach (var config in mappingConfig.MappingTables)
            {
                config.SouValue = Convert.ToString(jsonObject.SelectToken(config.SouPropertyPath)) ?? string.Empty;

                var propertyInfo = typeof(T).GetProperty(config.DesProperty);
                var getType = DataSourceFactory.ParseDatatype(config.DesDatatype);
                var convertedValue = DataSourceFactory.GetValueHandler(getType, config.SouValue, mapping, jsonObject, config.SouPropertyPath);
                propertyInfo?.SetValue(obj, convertedValue);
            }

            return obj;
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
            var client = new HttpClient();
            using var response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}