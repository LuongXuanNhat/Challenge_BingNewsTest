using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.DataAccessLayer.Constants;
using BingNew.DataAccessLayer.Entities;
using BingNew.ORM.DbContext;
using BingNew.ORM.NonQuery;
using BingNew.ORM.Query;
using Dasync.Collections;
using System.Data.SqlClient;
using static Dapper.SqlMapper;

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
        private string CreateQueryString<T>(){
            return "SELECT * FROM " + typeof(T).Name;
        }
        public async Task<bool> AddAdvertisement(AdArticle ad)
        {
            using var connectionn = new SqlConnection(new ConstantCommon().connectString);
            await connectionn.OpenAsync();
            connectionn.Insert(ad);
            return true;
        }
        public async Task<bool> AddRangerAdver(List<AdArticle> ads)
        {
            foreach (var item in ads)
            {
               await AddAdvertisement(item);
            }
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
        public async Task<WeatherVm> GetWeatherForecast(DateTime now)
        {
            var weather = await GetWeatherInDay(now);
            var weatherInfor = await GetWeatherInforInDay(now, weather.Id);
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

        public async Task<Weather> GetWeatherInDay(DateTime date)
        {
            return await connection.QueryAsync<Weather>(CreateQueryString<Weather>())
                    .FirstAsync(x => x.PubDate.Date == date.Date);
        }

        public async Task<List<WeatherInfo>> GetWeatherInforInDay(DateTime date, Guid weatherId)
        {
            return await connection.QueryAsync<WeatherInfo>(CreateQueryString<WeatherInfo>())
                .Where(x => x.WeatherId == weatherId)
                .ToListAsync();
        }

        public List<Article> Search(string keyWord)
        {
            string[] searchKeywords = keyWord.ToLower().Split(' ');
            var query = from article in connection.Query<Article>(CreateQueryString<Article>())
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
        public async Task<List<Article>> FullTextSearch(string keyWord)
        {
            var sql = "SELECT * FROM Article WHERE FREETEXT((Title, Description)," + "'" + keyWord + "')";
            List<Article> result = await connection.QueryAsync<Article>(sql).ToListAsync();
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
            SqlExtensionNonQuery.Insert(connection, users);
            return true;
        }

        public bool AddUserInteraction(UserInteraction userInteraction)
        {
            connection.Insert<UserInteraction>(userInteraction);
            return true;
        }

        public bool DeleteUserInteraction(UserInteraction userInteraction)
        {
            connection.Delete<UserInteraction>(userInteraction.GetId());
            return true;
        }

        public bool AddUserClick(UserClickEvent userClick)
        {
            SqlExtensionNonQuery.Insert(connection, userClick);
            return true;
        }

        public async Task<List<Article>> Recommendation(Guid userId)
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

            var articles = await connection.QueryAsync<Article>(sql).ToListAsync();
            return articles;
        }

        public bool AddWeather(Weather weatherr)
        {
            connection.Insert<Weather>(weatherr); 
            return true;
        }

        public bool AddWeatherRanger(List<WeatherInfo> weatherInfor)
        {
            foreach (var item in weatherInfor)
            {
            connection.Insert<WeatherInfo>(item);
            }
            return true;
        }

        public List<AdArticle> GetAdArticles()
        {
            return connection.Query<AdArticle>(CreateQueryString<AdArticle>()).ToList();
        }
    }
}