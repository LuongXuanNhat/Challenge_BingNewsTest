using BingNew.Mapping.Interface;
using Newtonsoft.Json.Linq;

namespace BingNew.Mapping
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
            using HttpClient client = new();
            return client.GetStringAsync(Url).Result;
        }
        public List<T> ConvertDataToArticles<T>(Config config, List<CustomConfig> mapping) where T : new()
        {
            var articles = new List<T>();
            JObject jsonObject = config.Data != null ? JObject.Parse(config.Data)
                : throw new InvalidOperationException("Could not get data");

            var newsArray = jsonObject[config.Item] as JArray;

            foreach (JObject newsItem in newsArray?.OfType<JObject>() ?? Enumerable.Empty<JObject>())
            {
                var article = ConvertDataToType<T>(newsItem.ToString(), mapping);
                articles.Add(article);
            }

            return articles;
        }

        public T ConvertDataToType<T>(string data, List<CustomConfig> mapping) where T : new()
        {
            T obj = new();
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