using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.DataAccessLayer.Entities;
using BingNew.ORM.DbContext;

namespace BingNew.BusinessLogicLayer.Services
{
    public class BingNewsService : IBingNewsService
    {
        private readonly DbBingNewsContext _dataContext;
        public BingNewsService(DbBingNewsContext context) {
            _dataContext = context;
        }

        public Tuple<bool, string, List<Article>> GetTopNews(int quantity)
        {
            var articles = _dataContext.GetAll<Article>()
                        .Where(x => x.PubDate.Date == DateTime.Now.Date)
                        .OrderBy(x => x.LikeNumber + x.ViewNumber + x.CommentNumber * 2 + x.DisLikeNumber)
                        .Take(quantity)
                        .ToList();

            return new Tuple<bool, string, List<Article>>(true, "", articles);
        }

        public Tuple<bool, string, List<Article>> GetTrendingArticlesPanel(int quantity)
        {
            var articles = _dataContext.GetAll<Article>()
                        .Where(x => x.PubDate >= DateTime.Now.AddDays(-3))
                        .Take(quantity)
                        .ToList();

            return new Tuple<bool, string, List<Article>>(true, "", articles);
        }
        public Tuple<bool, string, WeatherVm> GetWeatherForecast(DateTime now)
        {
            var weather = GetWeatherInDay(now);
            var weatherInfor = GetWeatherInforInDay(now, weather.Id);
            var weatherVm = new WeatherVm()
            {
                Id = weather.Id,
                Icon = weather.Icon,
                Description = weather.Description,
                Humidity = weather.Humidity,
                Place = weather.Place,
                PubDate = weather.PubDate,
                Temperature = weather.Temperature,
                WeatherInfor = weatherInfor,
            };
            return new Tuple<bool, string, WeatherVm>(true, "", weatherVm);
         
        }

        public Weather GetWeatherInDay(DateTime date)
        {
            return _dataContext.GetAll<Weather>()
                    .First(x => x.PubDate.Date == date.Date);
        }

        public List<WeatherInfo> GetWeatherInforInDay(DateTime date, Guid weatherId)
        {
            return _dataContext.GetAll<WeatherInfo>()
                .Where(x => x.WeatherId == weatherId)
                .ToList();
        }
    }
}