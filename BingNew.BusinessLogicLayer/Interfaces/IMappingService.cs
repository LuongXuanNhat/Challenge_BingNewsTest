using BingNew.BusinessLogicLayer.Services.Common;

namespace BingNew.BusinessLogicLayer.Interfaces
{
    public interface IMappingService
    {
        Tuple<bool, string> CrawlNewsXml(List<CustomConfig> customs);
        Tuple<bool, string> CrawlNewsJson(List<CustomConfig> customs);
    }
}
