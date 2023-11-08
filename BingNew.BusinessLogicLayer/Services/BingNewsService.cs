using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.DataAccessLayer.Entities;
using BingNew.ORM.DbContext;
using BingNew.ORM.NonQuery;
using BingNew.ORM.Query;
using System.Data.SqlClient;

namespace BingNew.BusinessLogicLayer.Services
{
    public class BingNewsService : IBingNewsService
    {
        private readonly DbBingNewsContext _dataContext;
        private readonly SqlConnection connection;
        public BingNewsService(DbBingNewsContext context) {
            _dataContext = context;
            connection = context.CreateConnection();
        }

        public bool AddAdvertisement(AdArticle ad)
        {
            connection.Insert(ad);
            return true;
        }

        public List<Article> GetTopNews(int quantity)
        {
            var articles = _dataContext.GetAll<Article>()
                        .Where(x => x.PubDate.Date == DateTime.Now.Date)
                        .OrderBy(x => x.LikeNumber + x.ViewNumber + x.CommentNumber * 2 + x.DisLikeNumber)
                        .Take(quantity)
                        .ToList();

            return articles;
        }

        public List<Article> GetTrendingArticlesPanel(int quantity)
        {
            var articles = _dataContext.GetAll<Article>()
                        .Where(x => x.PubDate >= DateTime.Now.AddDays(-3))
                        .OrderBy(x => x.LikeNumber + x.ViewNumber + x.CommentNumber * 2 + x.DisLikeNumber)
                        .Take(quantity)
                        .ToList();

            return articles;
        }
        public List<Article> GetTrendingArticlesPanel(int quantity, int numberBackDay)
        {
            return _dataContext.GetAll<Article>().Where(x => x.PubDate >= DateTime.Now.AddDays(-numberBackDay))
                        .OrderBy(x => x.LikeNumber + x.ViewNumber + x.CommentNumber * 2 + x.DisLikeNumber)
                        .Take(quantity).ToList();

        }
        public WeatherVm GetWeatherForecast(DateTime now)
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
            return weatherVm;
         
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

        public List<Article> Search(string keyWord)
        {
            string[] searchKeywords = keyWord.ToLower().Split(' ');
            var sql = "SELECT * FROM " + typeof(Article).Name;
            var query = from article in connection.Query<Article>(sql)
                        let titleWords = article.Title.Split(' ')
                        let descriptionWords = article.Description.Split(' ')
                        let searchPhrases = GenerateSearchPhrases(searchKeywords)
                        let matchingPhrases = searchPhrases
                            .Where(phrase => titleWords.Contains(phrase) || descriptionWords.Contains(phrase))
                        where matchingPhrases.Any()
                        let matchCount = matchingPhrases.Count()
                        orderby matchCount descending
                        select new Article()
                        {
                            CommentNumber = article.CommentNumber,
                            Title = article.Title,
                            Description = article.Description,
                            DisLikeNumber = article.DisLikeNumber,
                            Id = article.Id,
                            ImgUrl = article.ImgUrl,
                            LikeNumber = article.LikeNumber,
                            ChannelName = article.ChannelName,
                            PubDate = article.PubDate,
                            TopicId = article.TopicId,
                            Url = article.Url,
                            ViewNumber = article.ViewNumber,
                        };
            var searchResults = query.ToList();
            return searchResults;
        }
        public List<Article> FullTextSearch(string keyWord)
        {
            var sql = "SELECT * FROM Article WHERE FREETEXT((Title, Description),"+"'" + keyWord + "')";
            List<Article> result = connection.Query<Article>(sql).ToList();
            return result;
        }
        private static IEnumerable<string> GenerateSearchPhrases(string[] searchKeywords)
        {
            List<string> searchPhrases = new();
            for (int i = searchKeywords.Length; i >= 1; i--)
            {
                for (int j = 0; j <= searchKeywords.Length - i; j++)
                {
                    string phrase = string.Join(" ", searchKeywords.Skip(j).Take(i));
                    searchPhrases.Add(phrase);
                }
            }
            return searchPhrases;
        }

        public bool RegisterUser(Users users)
        {
            _dataContext.Add(users);
            return true;
        }

        public bool AddUserInteraction(UserInteraction userInteraction)
        {
            _dataContext.Add(userInteraction);
            return true;
        }

        public bool DeleteUserInteraction(UserInteraction userInteraction)
        {
            connection.Delete<UserInteraction>(userInteraction.GetId());
            return true;
        }

        public bool AddUserClick(UserClickEvent userClick)
        {
            _dataContext.Add(userClick);
            return true;
        }

        public List<Article> Recommendation(Guid userId)
        {
            var sql = @"WITH TopChannels AS (
                    SELECT TOP 10 A.ChannelName, COUNT(UCE.ArticleId) AS Number_Click
                    FROM Article AS A
                    LEFT JOIN UserClickEvent AS UCE
                    ON A.Id = UCE.ArticleId AND UCE.UserId = '" + userId + @"' AND UCE.Date > DATEADD(DAY, -7, GETDATE())
                    GROUP BY A.ChannelName
                    ORDER BY Number_Click DESC
                )
                SELECT A.[Id], A.[Title], A.[ImgUrl], A.[Description], A.[PubDate]
                      , A.[Url], A.[LikeNumber], A.[DisLikeNumber], A.[ViewNumber]
                      , A.[CommentNumber], A.[ChannelName], A.[TopicId]
                FROM Article AS A
                INNER JOIN TopChannels AS TC
                ON A.ChannelName = TC.ChannelName
                AND CONVERT(DATE, A.PubDate) = CONVERT(DATE, GETDATE())";

            var articles = connection.Query<Article>(sql).ToList();
            return articles;
        }
    }
}