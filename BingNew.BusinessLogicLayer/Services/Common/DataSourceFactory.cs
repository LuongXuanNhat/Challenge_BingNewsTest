using BingNew.BusinessLogicLayer.ModelConfig;
using Newtonsoft.Json;

namespace BingNew.BusinessLogicLayer.Services.Common
{
    // Factory Method Design Pattern
    public static class DataSourceFactory
    {
        public enum DataSource
        {
            ApiDataSource,
            RssDataSource
        }
        public static List<MappingTable> CreateMapping(string jsonConfigMapping)
        {
            return JsonConvert.DeserializeObject<List<MappingTable>>(jsonConfigMapping) ?? new List<MappingTable>();
        }
    }
}
