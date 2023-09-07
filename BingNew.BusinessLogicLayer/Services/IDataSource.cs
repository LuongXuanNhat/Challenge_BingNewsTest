using BingNew.BusinessLogicLayer.ModelConfig;
using BingNew.DataAccessLayer.Models;

namespace BingNew.BusinessLogicLayer.Services;
public interface IDataSource
{
    public List<Article> ConvertDataToArticles(Config config, List<MappingTable> mapping);
    public string GetNews(string Url);
    public string GetWeatherInfor(Config config);
    public Weather ConvertDataToWeather(string data, List<MappingTable> mapping);
}