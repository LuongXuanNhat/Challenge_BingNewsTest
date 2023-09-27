using BingNew.DataAccessLayer.Models;
using Newtonsoft.Json.Linq;
using BingNew.BusinessLogicLayer.ModelConfig;
using System.Globalization;
using BingNew.DataAccessLayer.TestData;
using BingNew.BusinessLogicLayer.Interfaces;
using BingNew.BusinessLogicLayer.Interfaces.IService;

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
        private string DownloadJson(string Url)
        {
            using (HttpClient client = new HttpClient())
            {
                return client.GetStringAsync(Url).Result;
            }
        }
        private Article MapToArticle(JObject item, List<MappingTable> mapping)
        {
            var article = new Article();
            foreach (var obj in mapping)
            {
                try
                {
                    var sourceValue = item[obj.SouPropertyPath]?.ToString();
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
                        DateTime convertedValue = DateTime.Parse(dateString, culture);
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
        public List<Article> ConvertDataToArticles(Config config, List<MappingTable> mapping)
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

        private string GetDataWeather(HttpRequestMessage request)
        {
            var client = new HttpClient();
            using (var response = client.SendAsync(request).Result)
            {
                response.EnsureSuccessStatusCode();
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public Weather ConvertDataToWeather(string data, List<MappingTable> mapping)
        {
            var weather = new Weather();
            JObject jsonObject = JObject.Parse(data);
            foreach (var obj in mapping)
            {
                try
                {
                    var sourceValue = jsonObject.SelectToken(obj.SouPropertyPath)?.ToString();
                    obj.SouValue = sourceValue ?? string.Empty;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                var propertyInfo = typeof(Weather).GetProperty(obj.DesProperty);
                if (propertyInfo != null && propertyInfo.PropertyType != null)
                {
                    if (obj.SouDatatype == "string" && obj.DesDatatype == "DateTime")
                    {
                        string dateString = obj.SouValue;
                        dateString = dateString.Replace(" GMT+7", "");
                        CultureInfo culture = CultureInfo.InvariantCulture;
                        DateTime convertedValue = DateTime.Parse(dateString, culture);
                        propertyInfo.SetValue(weather, convertedValue);
                    }
                    else if (obj.SouDatatype == "string" && obj.DesDatatype == "List<WeatherInfo>")
                    {
                        var TestData = new DataSample();
                        var weatherInfoMappingConfig = DataSourceFactory.CreateMapping(TestData.GetWeatherInforMappingConfiguration());

                        var hourlyWeatherList = jsonObject.SelectToken(obj.SouPropertyPath);
                        if (hourlyWeatherList != null)
                        {
                            var weatherInfors = new List<WeatherInfo>();
                            foreach (var item in hourlyWeatherList)
                            {
                                weatherInfors.Add(ConvertDataToWeatherInfor(item.ToString(), weatherInfoMappingConfig, weather.GetId()));
                            }
                            propertyInfo.SetValue(weather, weatherInfors);
                        }

                    }
                    else
                    {
                        var convertedValue = Convert.ChangeType(obj.SouValue, propertyInfo.PropertyType);
                        propertyInfo.SetValue(weather, convertedValue);
                    }
                }
            }

            return weather;
        }
        private WeatherInfo ConvertDataToWeatherInfor(string data, List<MappingTable> weatherInfoMappingConfig, Guid id)
        {
            var weatherInHour = new WeatherInfo();
            JObject jsonObject = JObject.Parse(data);
            foreach (var obj in weatherInfoMappingConfig)
            {
                try
                {
                    var sourceValue = jsonObject.SelectToken(obj.SouPropertyPath)?.ToString();
                    obj.SouValue = sourceValue ?? string.Empty;

                    var propertyInfo = typeof(WeatherInfo).GetProperty(obj.DesProperty);
                    if (propertyInfo != null && propertyInfo.PropertyType != null)
                    {
                        if (obj.SouDatatype == "string" && obj.DesDatatype == "DateTime.Hour")
                        {
                            string dateString = obj.SouValue;
                            dateString = dateString.Replace(" GMT+7", "");
                            CultureInfo culture = CultureInfo.InvariantCulture;
                            DateTime convertedValue = DateTime.Parse(dateString, culture);
                            propertyInfo.SetValue(weatherInHour, convertedValue.Hour);
                        }
                        else
                        {
                            var convertedValue = Convert.ChangeType(obj.SouValue, propertyInfo.PropertyType);
                            propertyInfo.SetValue(weatherInHour, convertedValue);
                        }
                    }
                    weatherInHour.SetWeatherId(id);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return weatherInHour;
        }
    }
}