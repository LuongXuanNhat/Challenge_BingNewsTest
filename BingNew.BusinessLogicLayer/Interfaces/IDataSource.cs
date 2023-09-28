using BingNew.BusinessLogicLayer.ModelConfig;
using BingNew.BusinessLogicLayer.Services.Common;
using BingNew.DataAccessLayer.Entities;

namespace BingNew.BusinessLogicLayer.Interfaces;
public interface IDataSource
{
    public List<Article> ConvertDataToArticles(Config config, List<CustomConfig> mapping);
    public string GetNews(string Url);
    public string GetWeatherInfor(Config config);
    public Weather ConvertDataToWeather(string data, List<CustomConfig> mapping);
}