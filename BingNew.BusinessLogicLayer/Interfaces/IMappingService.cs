using BingNew.Mapping;

namespace BingNew.BusinessLogicLayer.Interfaces;

public interface IMappingService
{
    bool CrawlNewsXml(List<CustomConfig> customs);
    bool CrawlNewsJson(List<CustomConfig> customs);
    bool CrawlWeatherForecast(List<CustomConfig> customs);
}
