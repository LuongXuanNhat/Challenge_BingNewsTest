
public class NewsAggregationTest
{
    [Fact]
    public void TestNewsService()
    {
        var service = new NewsService();
        Assert.NotNull(service);
    }

    [Fact] 
    public void TestGetNewsTrend() {
        var service = new NewsService();
        var structure = new Structure();

        structure.Type = service.GetTypeRss();
        structure.Url = service.GetUrlNewsTrend();
        var result = service.GetArticles(structure);

        Assert.NotNull(result);
    }

    [Fact]
    public void TestGetNewsTrendFromNewsDataIo()
    {
        var service = new NewsService();
        var structure = new Structure();
        structure.Type = "NewsDataIo";
        structure.KeyWork = "&q=" + "trend";
        structure.Language = "&language=" + "vi";
        structure.Key = "apikey=" + "pub_2815763c25cffe45251bb8682ef275560ee69";
        structure.Url = "https://newsdata.io/api/1/news?"
            + structure.Key
            + structure.KeyWork
            + structure.Language;

        var result = service.GetArticles(structure);

        Assert.NotNull(result);
    }

    [Fact]
    public void TestGetNewsFromBingNewsSearch()
    {
        var service = new NewsService();
        var structure = new Structure();
        structure.Type = "api";
        structure.Language = "vi";
        structure.Url = "https://bing-news-search1.p.rapidapi.com/news?safeSearch=Off&textFormat=Raw";
        structure.Headers.RapidApiKey = "63e013be17mshfaa183691e3f9fap12264bjsn8690697c78c9";
        structure.Headers.RapidApiHost = "bing-news-search1.p.rapidapi.com";

        var result = service.GetArticles(structure);

        Assert.NotNull(result);
    }

    [Fact]
    public void TestGetNewsTrendFromBingNewsSearch()
    {
        var service = new NewsService();
        var structure = new Structure();
        structure.Type = "api";
        structure.Language = "vi";
        structure.Url = "https://bing-news-search1.p.rapidapi.com/news/trendingtopics?textFormat=Raw&safeSearch=Off";
        structure.Headers.RapidApiKey = "63e013be17mshfaa183691e3f9fap12264bjsn8690697c78c9";
        structure.Headers.RapidApiHost = "bing-news-search1.p.rapidapi.com";

        var result = service.GetArticles(structure);

        Assert.NotNull(result);
    }

    //[Fact]
    //public void TestGetNewsFromQuickStart()
    //{
    //    var service = new NewsService();
    //    var structure = new Structure();
    //    structure.Type = "api";
    //    structure.Language = "vi";
    //    structure.Key = "3862de56187e9dab37ae52b71cdd881c";
    //    structure.Url = "http://api.mediastack.com/v1/news" + "?access_key=" + structure.Key;
        
    //    var result = service.GetArticles(structure);

    //    Assert.NotNull(result);
    //}

    [Fact]
    public void TestGetNewsByCategriesFromNewsDataIo()
    {
        var service = new NewsService();
        var structure = new Structure();
        structure.Type = "NewsDataIo"; 
        structure.Key =      "apikey=" + "pub_2815763c25cffe45251bb8682ef275560ee69";
        structure.Language = "&language=" + "vi";
        structure.Category = "&category=" + "business,entertainment";
        structure.Url =      "https://newsdata.io/api/1/news?"
                             + structure.Key
                             + structure.Language
                             + structure.Category;

        var result = service.GetArticles(structure);

        Assert.NotNull(result);
    }


}