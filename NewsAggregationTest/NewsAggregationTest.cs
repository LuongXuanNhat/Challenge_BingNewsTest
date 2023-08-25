
public class NewsAggregationTest
{
    [Fact]
    public void TestNewsService()
    {
        var service = new NewsService();
        Assert.NotNull(service);
    }

    [Fact] 
    public void TestGetNewsByRssTuoiTreNews()
    {
        IDataSource dataSource = new RssDataSource();
        var service = new NewsService();
        var config = new Config();
        config.Url = "https://tuoitre.vn/rss/the-gioi.rss";
        config.Type = "World";

        var result = dataSource.GetArticles(config);

        Assert.NotNull(result);
    }

    [Fact] 
    public void TestGetNewsByRssGoogleTrend()
    {
        IDataSource dataSource = new RssDataSource();
        var service = new NewsService();
        var config = new Config();
        config.Url = "https://trends.google.com.vn/trends/trendingsearches/daily/rss?geo=VN";
        config.Type = "Trend";
        config.NameSpace = "https://trends.google.com.vn/trends/trendingsearches/daily";

        var result = dataSource.GetArticles(config);

        Assert.NotNull(result);
    }

    [Fact]
    public void TestGetNewsByNewsDataIo()
    {
        IDataSource datasources = new ApiDataSource();
        var service = new NewsService();
        var config = new Config();
        config.Key = "apikey=" + "pub_2815763c25cffe45251bb8682ef275560ee69";
        config.Language = "&language=" + "vi";
        config.Category = "&category=" + "business,entertainment";
        config.Url = "https://newsdata.io/api/1/news?"
                             + config.Key
                             + config.Language
                             + config.Category;

        var result = datasources.GetArticles(config);

        Assert.NotNull(result);
    }

    [Fact]
    public void TestGetNewsByNewsApi()
    {
        IDataSource datasources = new ApiDataSource();
        var service = new NewsService();
        var config = new Config();
        config.Key = "&apikey=" + "6cbcb9e942954f92a54c65e3714ec500";
        config.Language = "&language=" + "vi";
        config.Country = "country=" + "us";
        config.Category = "&category=" + "entertainment";
        config.Url = "https://newsapi.org/v2/top-headlines?"
                             + config.Country
                             + config.Key;

        var result = datasources.GetArticles(config);

        Assert.NotNull(result);
    }

}