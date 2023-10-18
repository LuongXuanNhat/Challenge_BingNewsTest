using System;
using System.Xml.Linq;
using BingNew.Mapping.Interface;

namespace BingNew.Mapping
{
    // Strategy Pattern 
    public class XmlDataSource : IXmlDataSource 
    {
        public XmlDataSource()
        {

        }
        private readonly Dictionary<MappingType, IChooseMapping> MappingHandlers = new()
        {
            { MappingType.Single, new SingleMapDataXml() },
            { MappingType.List, new ListMapDataXml() }
        };

        public string FetchData(Config config)
        {
            using HttpClient client = new();
            return client.GetStringAsync(config.Url).Result;
        }

        public Tuple<bool, IEnumerable<object>, string> MapMultipleObjects(List<CustomConfig> customConfigs)
        {
            List<object> mappedDataList = new();
            foreach (var customConfig in customConfigs)
            {
                var rawData = FetchData(customConfig.Config);
                MappingHandlers.TryGetValue(customConfig.SingleMappingOrListMapping, out var handler);
                var obj = handler is not null ? handler.MapData(customConfig, rawData)
                                    : throw new NotSupportedException("Datatype not supported");
                mappedDataList.Add(obj);
            }
            return Tuple.Create(true, (IEnumerable<object>)mappedDataList, "");
        }
    }
}