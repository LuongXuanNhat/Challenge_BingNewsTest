using BingNew.DataAccessLayer.Entities;
using System.Collections;

namespace BingNew.BusinessLogicLayer.Interfaces.IService
{
    public interface IBingNewsService
    {
        List<Article> GetTopNews(int quantity);
        List<Article> GetTrendingArticlesPanel(int quantity);
        Weather GetWeatherInDay(DateTime date);
        List<WeatherInfo> GetWeatherInforInDay(DateTime date, Guid weatherId);
    }
}