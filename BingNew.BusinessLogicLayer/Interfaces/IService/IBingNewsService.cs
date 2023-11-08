using BingNew.DataAccessLayer.Entities;
using System.Collections;

namespace BingNew.BusinessLogicLayer.Interfaces.IService
{
    public interface IBingNewsService
    {
        bool AddAdvertisement(AdArticle ad);
        bool AddUserClick(UserClickEvent userClick);
        bool AddUserInteraction(UserInteraction userInteraction);
        bool DeleteUserInteraction(UserInteraction userInteraction);
        List<Article> FullTextSearch(string keyWord);
        List<Article> GetTopNews(int quantity);
        List<Article> GetTrendingArticlesPanel(int quantity); 
        List<Article> GetTrendingArticlesPanel(int quantity, int numberBackDay);
        WeatherVm GetWeatherForecast(DateTime now);
        Weather GetWeatherInDay(DateTime date);
        List<WeatherInfo> GetWeatherInforInDay(DateTime date, Guid weatherId);
        List<Article> Recommendation(Guid userId);
        bool RegisterUser(Users users);
        List<Article> Search(string keyWord);

    }
}