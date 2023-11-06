using BingNew.DataAccessLayer.Entities;
namespace BingNew.BusinessLogicLayer.Interfaces.IService
{
    public interface IBingNewsService
    {
        bool AddAdvertisement(AdArticle ad);
        bool AddUserInteraction(UserInteraction userInteraction);
        bool DeleteUserInteraction(UserInteraction userInteraction);
        List<Article> FullTextSearch(string keyWord);
        List<Article> GetTopNews(int quantity);
        List<Article> GetTrendingArticlesPanel(int quantity); 
        List<Article> GetTrendingArticlesPanel(int quantity, int numberBackDay);
        WeatherVm GetWeatherForecast(DateTime now);
        Weather GetWeatherInDay(DateTime date);
        List<WeatherInfo> GetWeatherInforInDay(DateTime date, Guid weatherId);
        bool RegisterUser(Users users);
        List<Article> Search(string keyWord);

    }
}