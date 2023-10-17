using System;
using System.Xml.Linq;
using BingNew.Mapping.Interface;

namespace BingNew.Mapping
{
    //// Strategy Pattern 
    public class RssDataSource : IRssDataSource 
    {


        public RssDataSource()
        {

        }
        private readonly Dictionary<SingleOrList, IChooseMapping> ChooseType = new()
        {
            { SingleOrList.Single, new SingleMapDataXml() },
            { SingleOrList.List, new ListMapDataXml() }
        };

        public string GetData(Config config)
        {
            using HttpClient client = new();
            return client.GetStringAsync(config.Url).Result;
        }

        public Tuple<bool, IEnumerable<object>, string> MultipleMapping(List<CustomConfig> customConfigs)
        {
            List<object> listData = new();
            foreach (var itemData in customConfigs)
            {
                var dataHandle = GetData(itemData.Config);
                ChooseType.TryGetValue(itemData.SingleMappingOrListMapping, out var handler);
                var obj = handler is not null ? handler.HandleData(itemData, dataHandle)
                                    : throw new NotSupportedException("Datatype not supported");
                listData.Add(obj);
            }
            return Tuple.Create(true, (IEnumerable<object>)listData, " ");
        }
    }
}