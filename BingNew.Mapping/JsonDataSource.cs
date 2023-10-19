using BingNew.Mapping.Interface;
namespace BingNew.Mapping
{
    // Strategy Pattern 
    public class JsonDataSource : IJsonDataSource
    {
        public JsonDataSource()
        {
        }

        private readonly Dictionary<MappingType, IChooseMapping> MappingHandlers = new()
        {
            { MappingType.Single, new SingleMapDataJson() },
            { MappingType.List, new ListMapDataJson() }
        };
        public IEnumerable<object> MapMultipleObjects(List<CustomConfig> customConfigs)
        {
            List<object> list = new();
            foreach (var item in customConfigs)
            {
                var data = FetchData(item.Config);
                MappingHandlers.TryGetValue(item.SingleMappingOrListMapping, out var handler);
                var obj =  handler?.MapData(item, data) ?? "undefined type";
                list.Add(obj);
            }
            return list;
        }
      
        public string FetchData(Config config)
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