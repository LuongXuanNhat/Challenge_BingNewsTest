using BingNew.DataAccessLayer.Entities;
namespace BingNew.BusinessLogicLayer.Interfaces.IService
{
    public interface IBingNewsService
    {
        Tuple<bool, string, List<Article>> GetTopNews(int quantity);
        Tuple<bool, string,List<Article>> GetTrendingArticlesPanel(int quantity);
        Tuple<bool, string, WeatherVm> GetWeatherForecast(DateTime now);
        Weather GetWeatherInDay(DateTime date);
        List<WeatherInfo> GetWeatherInforInDay(DateTime date, Guid weatherId);
    }
}