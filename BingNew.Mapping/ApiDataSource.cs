using BingNew.Mapping.Interface;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

namespace BingNew.Mapping
{
    //// Strategy Pattern 
    public class ApiDataSource : IApiDataSource
    {
        public ApiDataSource()
        {
        }
        private readonly Dictionary<SingleOrList, IChooseMapping> ChooseType = new()
        {
            { SingleOrList.Single, new SingleMapData() },
            { SingleOrList.List, new ListMapData() }
        };
        public Tuple<bool, IEnumerable<object>, string> MultipleMapping(List<CustomConfig> customConfigs)
        {
            ////try {
                List<object> list = new();
                foreach (var item in customConfigs)
                {
                    var data = GetData(item.Config);
                    ChooseType.TryGetValue(item.SingleMappingOrListMapping, out var handler);
                    var obj = handler is not null ? handler.HandleData(item, data)
                                        : throw new NotSupportedException("Datatype not supported");
                    list.Add(obj);
                }
                return Tuple.Create(true, (IEnumerable<object>)list, " ");
            ////}
            ////catch (Exception ex)
            ////{
            ////    return Tuple.Create(false, (IEnumerable<object>) new List<object>(), "Lỗi: " + ex.Message);
            ////}
        }
      
        public string GetData(Config config)
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