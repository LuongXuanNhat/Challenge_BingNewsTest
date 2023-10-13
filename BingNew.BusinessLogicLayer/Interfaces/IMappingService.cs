using BingNew.Mapping;

namespace BingNew.BusinessLogicLayer.Interfaces;

public interface IMappingService
{
    Tuple<bool, string> CrawlNewsXml(List<CustomConfig> customs);
    Tuple<bool, string> CrawlNewsJson(List<CustomConfig> customs);
    Tuple<bool, string> CrawlWeatherForecast(List<CustomConfig> customs);
}
