using BingNew.DataAccessLayer.Entities;
using System.Collections;

namespace BingNew.BusinessLogicLayer.Interfaces.IService
{
    public interface IBingNewsService
    {
        Task<bool> AddAdvertisement(AdArticle ad);
        Task<bool> AddRangerAdver(List<AdArticle> ads);
        bool AddUserClick(UserClickEvent userClick);
        bool AddUserInteraction(UserInteraction userInteraction);
        bool AddWeather(Weather weatherr);
        bool AddWeatherRanger(List<WeatherInfo> weatherInfor);
        bool DeleteUserInteraction(UserInteraction userInteraction);
        Task<List<Article>> FullTextSearch(string keyWord);
        List<AdArticle> GetAdArticles();
        List<Article> GetTopNews(int quantity);
        List<Article> GetTrendingArticlesPanel(int quantity); 
        List<Article> GetTrendingArticlesPanel(int quantity, int numberBackDay);
        Task<WeatherVm> GetWeatherForecast(DateTime now);
        Task<Weather> GetWeatherInDay(DateTime date);
        Task<List<WeatherInfo>> GetWeatherInforInDay(DateTime date, Guid weatherId);
        Task<List<Article>> Recommendation(Guid userId);
        bool RegisterUser(Users users);
        List<Article> Search(string keyWord);

    }
}