using BingNew.BusinessLogicLayer.Interfaces;
using BingNew.BusinessLogicLayer.ModelConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
