using BingNew.BusinessLogicLayer.ModelConfig;
using BingNew.DataAccessLayer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Linq;
using System.Text.Json.Nodes;

namespace BingNew.BusinessLogicLayer.Services.Common
{
    public class NewsService
    {

        public NewsService()
        {

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

        public List<MappingTable> CreateMapping(string jsonConfigMapping)
        {
            return JsonConvert.DeserializeObject<List<MappingTable>>(jsonConfigMapping) ?? new List<MappingTable>();
        }



    }
}