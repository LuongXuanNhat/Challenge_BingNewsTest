using BingNew.DataAccessLayer.Entities;
namespace BingNew.BusinessLogicLayer.Interfaces.IService
{
    public interface IBingNewsService
    {
        bool AddAdvertisement(AdArticle ad);
        List<Article> FullTextSearch(string keyWord);
        List<Article> GetTopNews(int quantity);
        List<Article> GetTrendingArticlesPanel(int quantity);
        WeatherVm GetWeatherForecast(DateTime now);
        Weather GetWeatherInDay(DateTime date);
        List<WeatherInfo> GetWeatherInforInDay(DateTime date, Guid weatherId);
        List<Article> Search(string keyWord);
    }
}