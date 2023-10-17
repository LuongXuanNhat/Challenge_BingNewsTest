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
            ////try
            ////{
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
            ////    return Tuple.Create(false, (IEnumerable<object>)new List<object>(), "Lỗi: " + ex.Message);
            ////}
        }
    }
}