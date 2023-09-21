using AutoFixture;
using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.BusinessLogicLayer.ModelConfig;
using BingNew.BusinessLogicLayer.Services.Common;
using BingNew.DataAccessLayer.Constants;
using BingNew.DataAccessLayer.Models;
using BingNew.DataAccessLayer.TestData;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace NewsAggregationTest
{
    public class BingNewsTest
    {
        private readonly NewsService _newsService;
        private readonly DataSample _dataSample;
        private readonly Config _config;
        private readonly IFixture _fixture;
        private readonly string _connecString;
        private readonly IApiDataSource _apiDataSource;
        private readonly IRssDataSource _rssDataSource;
        ////private readonly IArticleService _articleService;
        ////private readonly IMappingService _mappingService;
        ////private readonly IWeatherService _weatherService;

        public BingNewsTest()
        {
            _dataSample = new DataSample();
            _newsService = new NewsService();
            _config = new Config();
            _fixture = new Fixture();
            _apiDataSource = new ApiDataSource();
            _rssDataSource = new RssDataSource();
            _connecString = new ConstantCommon().connectString;
        }

        private Config WeatherConfig()
        {
            var config = new Config();
            config.Headers.RapidApiKey = "63e013be17mshfaa183691e3f9fap12264bjsn8690697c78c9";
            config.Headers.RapidApiHost = "weatherapi-com.p.rapidapi.com";
            config.KeyWork = "q=" + "Ho Chi Minh";
            config.DayNumber = "&day=" + "3";
            config.Language = "&lang=" + "vi";
            config.Location = "location";
            config.Url = "https://weatherapi-com.p.rapidapi.com/forecast.json?" + config.KeyWork + config.DayNumber + config.Language;
            return config;
        }

        #region BingNews
        [Fact]
        public void CreateNewsServiceNotNull()
        {
            var service = new NewsService();
            Assert.NotNull(service);
        }

        [Fact]
        public void GetNewsFromRssTuoiTreNotNull()
        {
            var result = _rssDataSource.GetNews("https://tuoitre.vn/rss/tin-moi-nhat.rss");
            Assert.NotNull(result);
        }

        [Fact]
        public void GetNewsFromRssGoogleTrendNotNull()
        {
            var result = _rssDataSource.GetNews("https://trends.google.com.vn/trends/trendingsearches/daily/rss?geo=VN");
            Assert.NotNull(result);
        }

        [Fact]
        public void GetNewsFromApiNewsDataIoNotNull()
        {
            var config = new Config();
            config.Key = "apikey=" + "pub_2815763c25cffe45251bb8682ef275560ee69";
            config.Language = "&language=" + "vi";
            config.Item = "results";
            config.Url = "https://newsdata.io/api/1/news?" + config.Key + config.Language;

            var result = _apiDataSource.GetNews(config.Url);

            Assert.NotNull(result);
        }

        [Fact]
        public void ConvertDataFromTuoiTreNewsToArticlesNotNull()
        {
            _config.Data = _rssDataSource.GetNews("https://tuoitre.vn/rss/tin-moi-nhat.rss");
            _config.Item = "item";
            _config.Channel = "Tuoi Tre News";

            var mappingConfig = _newsService.CreateMapping(_dataSample.GetRssTuoiTreNewsDataMappingConfiguration());
            var result = _rssDataSource.ConvertDataToArticles(_config, mappingConfig);

            Assert.NotNull(result);
        }

        [Fact]
        public void ConvertDataFromGgTrendsToArticlesNotNull()
        {
            _config.Data = _rssDataSource.GetNews("https://trends.google.com.vn/trends/trendingsearches/daily/rss?geo=VN");
            _config.Item = "item";

            var mappingConfig = _newsService.CreateMapping(_dataSample.GetGgTrendsNewsDataMappingConfiguration());
            var result = _rssDataSource.ConvertDataToArticles(_config, mappingConfig);

            Assert.NotNull(result);
        }

        [Fact]
        public void ConvertDataFromNewsDataToArticlesNotNull()
        {
            _config.Key = "apikey=" + "pub_2815763c25cffe45251bb8682ef275560ee69";
            _config.Language = "&language=" + "vi";
            _config.Category = "&category=" + "business,entertainment";
            _config.Url = "https://newsdata.io/api/1/news?" + _config.Key + _config.Language + _config.Category;
            _config.Data = _apiDataSource.GetNews(_config.Url);
            _config.Item = "results";

            var mappingConfig = _newsService.CreateMapping(_dataSample.GetNewsDataIoMappingConfiguration());
            var result = _apiDataSource.ConvertDataToArticles(_config, mappingConfig);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetWeatherInforNotNull()
        {
            var service = new NewsService();
            var config = new Config();
            config.Headers.RapidApiKey = "63e013be17mshfaa183691e3f9fap12264bjsn8690697c78c9";
            config.Headers.RapidApiHost = "weatherapi-com.p.rapidapi.com";
            config.KeyWork = "q=" + "Ho Chi Minh";
            config.DayNumber = "&day=" + "3";
            config.Language = "&lang=" + "vi";
            config.Location = "location";
            config.Url = "https://weatherapi-com.p.rapidapi.com/forecast.json?" + config.KeyWork + config.DayNumber + config.Language;

            var result = service.GetWeatherInfor(config);
            Assert.NotNull(result);
        }

        [Fact]
        public void ConvertDataToWeatherNotNull()
        {
            var config = WeatherConfig();
            var weatherMappingConfig = _newsService.CreateMapping(_dataSample.GetWeatherMappingConfiguration());
            var data = _apiDataSource.GetWeatherInfor(config);
            var result = _apiDataSource.ConvertDataToWeather(data, weatherMappingConfig);

            Assert.NotNull(result);
            Assert.NotNull(result.GetHourlyWeather());
        }
        #endregion

        #region DI & Api
        ////[Fact]
        ////public async Task AddArticleToDatabaseSuccess()
        ////{
        ////    Article article = _fixture.Create<Article>();

        ////    var result = await _articleService.Add(article);

        ////    Assert.True(result);
        ////}

        ////[Theory]
        ////[InlineData("257b736d-8451-49b0-ac9d-20fbbf1e3e1b")]
        ////public async Task GetByArticleIdToDatabaseSuccess(string id)
        ////{
        ////    var result = await _articleService.GetById(id);

        ////    Assert.NotNull(result.GetTitle());
        ////    Assert.NotEmpty(result.GetDescription());
        ////}

        ////[Theory]
        ////[InlineData("257b736d-8451-49b0-ac9d-20fbbf1e3e1b")]
        ////public async Task UpdateArticleToDatabaseSuccess(string id)
        ////{
        ////    var newTitle = Guid.NewGuid().ToString();
        ////    var article = await _articleService.GetById(id);

        ////    article.SetTitle(newTitle);
        ////    var result = await _articleService.Update(article);
        ////    var newArticle = await _articleService.GetById(article.GetId().ToString());

        ////    Assert.True(result);
        ////    Assert.Equal(newArticle.GetTitle(), newTitle);
        ////}

        ////[Fact]
        ////public async Task GetAllArticleNotNull()
        ////{
        ////    var articles = await _articleService.GetAll();
        ////    Assert.NotNull(articles);
        ////    Assert.NotEmpty(articles);
        ////}

        ////[Fact]
        ////public async Task DeleteArticleToDatabaseSuccess()
        ////{
        ////    Article article = _fixture.Create<Article>();
        ////    await _articleService.Add(article);

        ////    var result = await _articleService.Delete(article.GetId().ToString());

        ////    Assert.True(result);
        ////}

        ////[Fact]
        ////public async Task AddArticleToDatabaseFromTuoiTreNews()
        ////{
        ////    IDataSource _dataSource = new RssDataSource();
        ////    _config.Data = _dataSource.GetNews("https://tuoitre.vn/rss/tin-moi-nhat.rss");
        ////    _config.Item = "item";
        ////    _config.Channel = "Tuoi Tre News";
        ////    var mappingConfig = _newsService.CreateMapping(_dataSample.GetRssTuoiTreNewsDataMappingConfiguration());
        ////    var articles = _dataSource.ConvertDataToArticles(_config, mappingConfig);

        ////    var result = await _articleService.AddRange(articles);

        ////    Assert.True(result);
        ////}
        ////[Fact]
        ////public async Task AddArticleToDatabaseFromNewDataIo()
        ////{
        ////    _config.Key = "apikey=" + "pub_2815763c25cffe45251bb8682ef275560ee69";
        ////    _config.Language = "&language=" + "vi";
        ////    _config.Category = "&category=" + "business,entertainment";
        ////    _config.Url = "https://newsdata.io/api/1/news?" + _config.Key + _config.Language + _config.Category;
        ////    _config.Data = _apiDataSource.GetNews(_config.Url);
        ////    _config.Item = "results";

        ////    var mappingConfig = _newsService.CreateMapping(_dataSample.GetNewsDataIoMappingConfiguration());
        ////    var articles = _apiDataSource.ConvertDataToArticles(_config, mappingConfig);
        ////    var result = await _articleService.AddRange(articles);

        ////    Assert.True(result);
        ////}

        ////[Fact]
        ////public async Task GetTrendingNews()
        ////{
        ////    var result = await _articleService.TrendingStories();
        ////    Assert.NotNull(result);
        ////}

        ////[Fact]
        ////public void TestMappingArticle()
        ////{
        ////    var article = _fixture.Create<Article>();

        ////    var result = _mappingService.Map<Article, ArticleVm>(article);
        ////    Assert.NotNull(result);
        ////}

        ////[Fact]
        ////public async Task AddWeatherToDatabaseFromApi()
        ////{
        ////    var config = WeatherConfig();
        ////    var weatherMappingConfig = _newsService.CreateMapping(_dataSample.GetWeatherMappingConfiguration());
        ////    var data = _apiDataSource.GetWeatherInfor(config);
        ////    var wearther = _apiDataSource.ConvertDataToWeather(data, weatherMappingConfig);

        ////    var result = await _weatherService.Add(wearther);

        ////    Assert.True(result);
        ////}

        #endregion

        #region ORM
        [Fact]
        public void GetArticleSuccess()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                connection.Open();
                var sql = "SELECT * FROM Article where Id = 'dfdab4e6-538b-4ef5-8b69-00f99a9ad6bf'";
                var result = connection.QuerySingle<Article>(sql);
                Assert.NotNull(result);
            }
        }

        [Fact]
        public void GetArticle_Success()
        {
            using (var connection = new SqlConnection(_connecString))
            {
                connection.Open();
                var sql = "SELECT * FROM Article where Id = 'dfdab4e6-538b-4ef5-8b69-00f99a9ad6bf'";
                var result = connection.QuerySingle(sql);
                Assert.NotNull(result);
            }
        }


        #endregion

    }

    public static class SqlConnectionExtensions
    {
        public static T QuerySingle<T> (this SqlConnection connection, string sql, int? commandTimeout = null,
            SqlParameter[] sqlParameters = null,
            IDbTransaction transaction = null)
            where T : class, new()
        {
            using (var command = connection.CreateCommand())
            {
                command.Connection = connection;
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                command.CommandTimeout = commandTimeout ?? 30;

                if (sqlParameters != null && sqlParameters.Length > 0)
                {
                    command.Parameters.AddRange(sqlParameters);
                }
                using (var reader = command.ExecuteReader())
                {
                    return reader.Read() ? ConvertToObject<T>(reader) : default(T);
                }
            }
        }

        public static dynamic QuerySingle(this SqlConnection connectionn, string sql)
        {
            using (var command = new SqlCommand(sql, connectionn))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // các thuộc tính và giá trị vào đối tượng này mà không cần biết trước kiểu dữ liệu cụ thể của các thuộc tính.
                        dynamic result = new System.Dynamic.ExpandoObject();
                        string tableName = GetTypeNameFromSql(sql); 
                        Type classType = Type.GetType("Namespace." + tableName); 
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            ((IDictionary<string, object>)result)[columnName] = reader[i];
                        }

                        return result;
                    }
                    else
                    {
                        throw new InvalidOperationException("No rows found");
                    }
                }
            }
        }

        public static string GetTypeNameFromSql(string sql)
        {
            var fromIndex = sql.IndexOf("FROM");
            if (fromIndex >= 0)
            {
                var afterFrom = sql.Substring(fromIndex + 5).Trim();
                var spaceIndex = afterFrom.IndexOf(' ');
                if (spaceIndex >= 0)
                {
                    return afterFrom.Substring(0, spaceIndex);
                }
                return afterFrom;
            }

            throw new InvalidOperationException("Invalid SQL query");
        }


        //public static dynamic QuerySingle(this SqlConnection connection, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        //{
        //    using (var command = new SqlCommand(sql, connection))
        //    {
        //        using (var reader = command.ExecuteReader())
        //        {
        //            if (reader.HasRows && reader.Read())
        //            {
        //                var typeName = ExtractTypeNameFromSql(sql);
        //                if (!string.IsNullOrEmpty(typeName))
        //                {
        //                    var resultType = FindTypeByName(typeName);
        //                    if (resultType != null)
        //                    {
        //                        var obj = Activator.CreateInstance(resultType);

        //                        foreach (var propertyInfo in resultType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
        //                        {
        //                            var columnName = propertyInfo.Name; // Column name matches property name by default
        //                            if (reader.HasColumn(columnName) && !reader.IsDBNull(reader.GetOrdinal(columnName)))
        //                            {
        //                                var propertyValue = reader[columnName];
        //                                propertyInfo.SetValue(obj, propertyValue);
        //                            }
        //                        }

        //                        return obj;
        //                    }
        //                }
        //                return null;
        //            }
        //        }
        //    }
        //    return null;
        //}
        public static object QuerySingle2(this SqlConnection connection, string sql, SqlParameter[] parameters = null)
        {
            using (var command = new SqlCommand(sql, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows && reader.Read())
                    {
                        var typeName = ExtractTypeNameFromSql(sql);
                        if (!string.IsNullOrEmpty(typeName))
                        {
                            var resultType = FindTypeByName(typeName);
                            if (resultType != null)
                            {
                                var obj = Activator.CreateInstance(resultType);

                                foreach (var propertyInfo in resultType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                                {
                                    var columnName = propertyInfo.Name; // Column name matches property name by default
                                    if (reader.HasColumn(columnName) && !reader.IsDBNull(reader.GetOrdinal(columnName)))
                                    {
                                        var propertyValue = reader[columnName];
                                        propertyInfo.SetValue(obj, propertyValue);
                                    }
                                }

                                return obj;
                            }
                        }
                        return null;
                    }
                }
            }
            return null;
        }

        private static Type? FindTypeByName(string typeName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var type = assembly.GetType(typeName);
                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }

        public static string ExtractTypeNameFromSql(string sql)
        {
            var tableNameStartIndex = sql.IndexOf("FROM ", StringComparison.OrdinalIgnoreCase);
            if (tableNameStartIndex >= 0)
            {
                tableNameStartIndex += 5; // FROM 
                var tableNameEndIndex = sql.IndexOf(" ", tableNameStartIndex, StringComparison.OrdinalIgnoreCase);
                if (tableNameEndIndex > tableNameStartIndex)
                {
                    return sql.Substring(tableNameStartIndex, tableNameEndIndex - tableNameStartIndex);
                }
            }
            return null;
        }

        public static T ConvertToObject<T>(IDataReader reader) where T : new()
        {
            var obj = new T();
            var type = typeof(T);
            var properties = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance );

            foreach (var property in properties)
            {
                var columnName = property.Name; // get field name
                if (reader.HasColumn(columnName) && !reader.IsDBNull(reader.GetOrdinal(columnName)))
                {
                    var propertyValue = reader[columnName];
                    property.SetValue(obj, propertyValue);
                }
            }

            return obj;
        }

        public static bool HasColumn(this IDataReader reader, string columnName)
        {
            for (var i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }


}