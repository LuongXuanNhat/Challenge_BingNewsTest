
public class NewsAggregationTest
{
    [Fact]
    public void TestNewsService()
    {
        var service = new NewsService();
        Assert.NotNull(service);
    }

    [Fact] 
    public void TestGetNewsTrendFromGoogleTrend() {
        IDataSource datasources = new RssDataSource();
        var service = new NewsService();
        var config = new Config();

        config.Type = service.GetTypeRssGoogleTrend();
        config.Url = service.GetUrlNewsTrend();
        var result = datasources.GetNews(config);

        Assert.NotNull(result);
    }

    [Fact]
    public void TestGetNewsTrendFromNewsDataIo()
    {
        IDataSource datasources = new ApiDataSource();
        var service = new NewsService();
        var config = new Config();
        config.Type = service.GetTypeApiNewDataIo();
        config.KeyWork = "&q=" + "trend";
        config.Language = "&language=" + "vi";
        config.Key = "apikey=" + "pub_2815763c25cffe45251bb8682ef275560ee69";
        config.Url = "https://newsdata.io/api/1/news?"
            + config.Key
            + config.KeyWork
            + config.Language;

        var result = datasources.GetNews(config);

        Assert.NotNull(result);
    }

    //[Fact]
    //public void TestGetNewsFromBingNewsSearch()
    //{
    //    IDataSource datasources = new ApiDataSource();
    //    var service = new NewsService();
    //    var config = new Config();
    //    config.Type = service.GetTypeApiNewDataIo;
    //    config.Language = "vi";
    //    config.Url = "https://bing-news-search1.p.rapidapi.com/news?safeSearch=Off&textFormat=Raw";
    //    config.Headers.RapidApiKey = "63e013be17mshfaa183691e3f9fap12264bjsn8690697c78c9";
    //    config.Headers.RapidApiHost = "bing-news-search1.p.rapidapi.com";

    //    var result = datasources.GetNews(config);

    //    Assert.NotNull(result);
    //}

    //[Fact]
    //public void TestGetNewsTrendFromBingNewsSearch()
    //{
    //    IDataSource datasources = new ApiDataSource();
    //    var service = new NewsService();
    //    var structure = new Config();
    //    structure.Type = "api";
    //    structure.Language = "vi";
    //    structure.Url = "https://bing-news-search1.p.rapidapi.com/news/trendingtopics?textFormat=Raw&safeSearch=Off";
    //    structure.Headers.RapidApiKey = "63e013be17mshfaa183691e3f9fap12264bjsn8690697c78c9";
    //    structure.Headers.RapidApiHost = "bing-news-search1.p.rapidapi.com";

    //    var result = service.GetArticles(structure);

    //    Assert.NotNull(result);
    //}

    //[Fact]
    //public void TestGetNewsFromQuickStart()
    //{
    //    var service = new NewsService();
    //    var config = new Config();
    //    config.Type = "api";
    //    config.Language = "vi";
    //    config.Key = "3862de56187e9dab37ae52b71cdd881c";
    //    config.Url = "http://api.mediastack.com/v1/news" + "?access_key=" + config.Key;
        
    //    var result = service.GetArticles(config);

    //    Assert.NotNull(result);
    //}

    [Fact]
    public void TestGetNewsByCategriesFromNewsDataIo()
    {
        IDataSource datasources = new ApiDataSource();
        var service = new NewsService();
        var structure = new Config();
        structure.Type = service.GetTypeApiNewDataIo(); 
        structure.Key =      "apikey=" + "pub_2815763c25cffe45251bb8682ef275560ee69";
        structure.Language = "&language=" + "vi";
        structure.Category = "&category=" + "business,entertainment";
        structure.Url =      "https://newsdata.io/api/1/news?"
                             + structure.Key
                             + structure.Language
                             + structure.Category;

        var result = datasources.GetNews(structure);

        Assert.NotNull(result);
    }

    [Fact]
    public void TestGetNewsByChannel()
    {
        var service = new NewsService();
        var structure = new Config();
        IDataSource datasources = new RssDataSource();

        structure.Type = service.GetTypeRssGoogleTrend();
        structure.Url = service.GetUrlNewsTrend();
        var articles = datasources.GetNews(structure);

        var result = service.GetArticleByChannel(articles,"Tuổi Trẻ Online");

        Assert.NotNull(result);

    }

    [Fact]
    public void TestGetNews()
    {
        var service = new NewsService();
        var structure = new Config();
        IDataSource datasources = new RssDataSource();

        structure.Type = service.GetTypeRssGoogleTrend();
        structure.Url = service.GetUrlNewsTrend();
        var articles = datasources.GetNews(structure);

        var result = service.GetArticleByChannel(articles,"Tuổi Trẻ Online");

        Assert.NotNull(result);

    }
    [Fact]
    public void TestGetNewsRss()
    {
        var service = new NewsService();
        var structure = new Config();
        IDataSource datasources = new RssDataSource();

        structure.Type = service.GetTypeRssGoogleTrend();
        structure.Url = service.GetUrlNewsTrend();
        var articles = datasources.GetNews(structure);

        var result = service.GetArticleByChannel(articles,"Tuổi Trẻ Online");

        Assert.NotNull(result);
    }

}