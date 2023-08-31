
using BingNew.BusinessLogicLayer.Services;
using BingNew.DataAccessLayer.Models;


public class NewsAggregationTest
{
    [Fact]
    public void TestNewsService()
    {
        var service = new NewsService();
        Assert.NotNull(service);
    }
    private User Login()
    {
        return new User("1", "luongxuannhat", "email@gmail.com");
    }
    private List<Article> GetNewsByNewsDataIo()
    {
        IDataSource datasources = new ApiDataSource();
        var service = new NewsService();
        var config = new Config();
        config.Key = "apikey=" + "pub_2815763c25cffe45251bb8682ef275560ee69";
        config.Language = "&language=" + "vi";
        config.Item = "results";
        config.Url = "https://newsdata.io/api/1/news?"
                             + config.Key
                             + config.Language;
        config.MappingTable = new List<MappingTable>()
        {
            new MappingTable("title","Title"),
            new MappingTable("link", "Url") ,
            new MappingTable("description","Description" ),
            new MappingTable("pubDate", "PubDate"),
            new MappingTable("image_url", "ImgUrl"),
            new MappingTable("source_id", "Channel"),
            new MappingTable("category", "Category")
        };
        return datasources.GetNews(config);
    }
    private List<Article> GetNewsByGoogleTrend()
    {
        IDataSource dataSource = new RssDataSource();
        var service = new NewsService();
        var config = new Config();
        config.Url = "https://trends.google.com.vn/trends/trendingsearches/daily/rss?geo=VN";
        config.Type = "Trend";
        config.Item = "item";
        config.Channel = "Channel";
        config.NewsItems = "news_item";
        config.Namespace = "https://trends.google.com.vn/trends/trendingsearches/daily";
        config.MappingTable = new List<MappingTable>()
        {
            new MappingTable("title","Title"),
            new MappingTable("link", "Url") ,
            new MappingTable("description","Description" ),
            new MappingTable("pubDate", "PubDate"),
            new MappingTable("picture", "ImgUrl"),
            new MappingTable("picture_source", "Channel")

        };

        return dataSource.GetNews(config);
    }
    private List<Article> GetNewsByTuoiTreNews()
    {
        IDataSource dataSource = new RssDataSource();

        var service = new NewsService();
        var config = new Config();
        config.Url = "https://tuoitre.vn/rss/tin-moi-nhat.rss";
        config.Type = "Home";
        config.Item = "item";
        config.MappingTable = new List<MappingTable>()
        {
            new MappingTable("title","Title"),
            new MappingTable("link", "Url") ,
            new MappingTable("description","Description" ),
            new MappingTable("pubDate", "PubDate"),
            new MappingTable("image", "ImgUrl")
        };

        return dataSource.GetNews(config);
    }


    [Fact]
    public void TestGetNewsByRssTuoiTreNews()
    {
        IDataSource dataSource = new RssDataSource();

        var service = new NewsService();
        var config = new Config();
        config.Url = "https://tuoitre.vn/rss/the-gioi.rss";
        config.Type = "World";
        config.Item = "item";
        config.MappingTable = new List<MappingTable>()
        {
            new MappingTable("title","Title"),
            new MappingTable("link", "Url") ,
            new MappingTable("description","Description" ),
            new MappingTable("pubDate", "PubDate"),
            new MappingTable("image", "ImgUrl")
        };

        var result = dataSource.GetNews(config);

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
        config.Item = "item";
        config.Channel = "Channel";
        config.NewsItems = "news_item";
        config.Namespace = "https://trends.google.com.vn/trends/trendingsearches/daily";
        config.MappingTable = new List<MappingTable>()
        {
            new MappingTable("title","Title"),
            new MappingTable("link", "Url") ,
            new MappingTable("description","Description" ),
            new MappingTable("pubDate", "PubDate"),
            new MappingTable("picture", "ImgUrl"),
            new MappingTable("news_item_source", "Channel"),

        };

        var result = dataSource.GetNews(config);

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
        config.Item = "results";
        config.Url = "https://newsdata.io/api/1/news?"
                             + config.Key
                             + config.Language;
        config.MappingTable = new List<MappingTable>()
        {
            new MappingTable("title","Title"),
            new MappingTable("link", "Url") ,
            new MappingTable("description","Description" ),
            new MappingTable("pubDate", "PubDate"),
            new MappingTable("image_url", "ImgUrl"),
            new MappingTable("source_id", "Channel"),
            new MappingTable("category", "Category"),
        };


        var result = datasources.GetNews(config);

        Assert.NotNull(result);
    }

    //[Fact]
    //public void TestGetNewsByNewsApi()
    //{
    //    IDataSource datasources = new ApiDataSource();
    //    var service = new NewsService();
    //    var config = new Config();
    //    config.Key = "&apikey=" + "6cbcb9e942954f92a54c65e3714ec500";
    //    config.Language = "&language=" + "vi";
    //    config.Country = "country=" + "us";
    //    config.Category = "&category=" + "entertainment";
    //    config.Url = "https://newsapi.org/v2/top-headlines?"
    //                         + config.Country
    //                         + config.Key;
    //    config.MappingTable = new List<MappingTable>()
    //    {
    //        new MappingTable("title","Title"),
    //        new MappingTable("url", "Url") ,
    //        new MappingTable("description","Description" ),
    //        new MappingTable("publishedAt", "PubDate"),
    //        new MappingTable("urlToImage", "ImgUrl")
    //    };
    //    var result = datasources.GetNews(config);

    //    Assert.NotNull(result);
    //}

    [Fact]
    public void GetNewsByChannelFromNewsDataIo()
    {
        var service = new NewsService();
        var data = GetNewsByNewsDataIo();
        var channel = "kenh14";

        var result = service.GetDataByChannel(data, channel);

        Assert.NotNull(result);
    }

    [Fact]
    public void GetNewsByChannelFromGoogleTrend()
    {
        var service = new NewsService();
        var data = GetNewsByGoogleTrend();
        var channel = "Vietnamnet.vn";

        var result = service.GetDataByChannel(data, channel);

        Assert.NotNull(result);
    }

    [Fact]
    public void GetNewsByCategoryFromNewsDataIo()
    {
        IDataSource datasources = new ApiDataSource();
        var service = new NewsService();
        var config = new Config();
        config.Key = "apikey=" + "pub_2815763c25cffe45251bb8682ef275560ee69";
        config.Language = "&language=" + "vi";
        config.Category = "&category=" + "business,entertainment";
        config.Item = "results";
        config.Url = "https://newsdata.io/api/1/news?"
                             + config.Key
                             + config.Language
                             + config.Category;
        config.MappingTable = new List<MappingTable>()
        {
            new MappingTable("title","Title"),
            new MappingTable("link", "Url") ,
            new MappingTable("description","Description" ),
            new MappingTable("pubDate", "PubDate"),
            new MappingTable("image_url", "ImgUrl"),
            new MappingTable("source_id", "Channel"),
            new MappingTable("category", "Category")
        };
        var result = datasources.GetNews(config);

        Assert.NotNull(result);
    }

    [Fact]
    public void TestGetChannelFromGoogleTrend()
    {
        var service = new NewsService();
        var data = GetNewsByGoogleTrend();
        var channels = service.GetChannels(data);

        Assert.NotNull(channels);
    }


    [Fact]
    public void TestGetChannelFromNewsDataIo()
    {
        var service = new NewsService();
        var data = GetNewsByNewsDataIo();
        var channels = service.GetChannels(data);

        Assert.NotNull(channels);
    }

    [Fact]
    public void TestTrendingStoriesOfTuoiTreNews()
    {
        var service = new NewsService();
        var data = GetNewsByTuoiTreNews();
        var articleNumber = 10;
        var trendingNews = service.GetTopArticles(data, articleNumber);

        Assert.NotNull(trendingNews);
    }

    [Fact]
    public void TestTrendingStoriesOfNewsDataIo()
    {
        var service = new NewsService();
        var data = GetNewsByNewsDataIo();
        var articleNumber = 7;
        var trendingNews = service.GetTopArticles(data, articleNumber);

        Assert.NotNull(trendingNews);
    }

    [Fact]
    public void TestAddFavoriteChannelToFollowList()
    {
        var service = new NewsService();
        var data = GetNewsByGoogleTrend();
        var channels = service.GetChannels(data);
        var firstChannel = channels.First();

        var user = new User("1", "luongxuannhat", "email@gmail.com");
        var result = service.AddFavoriteChannel(user.Id, firstChannel);

        Assert.NotNull(result);
    }

    [Fact]
    public void TestRemoveFavoriteChannelFromFollowList()
    {
        var service = new NewsService();
        var data = GetNewsByGoogleTrend();
        var channels = service.GetChannels(data);
        var firstChannel = channels.First();
        var user = Login();
        var result = service.AddFavoriteChannel(user.Id, firstChannel);
        result = service.RemoveFavoriteChannel(user.Id, firstChannel, result);

        Assert.Empty(result);
    }



    [Fact]
    public void TestAddHateChannelToBlockedList()
    {
        var service = new NewsService();
        var data = GetNewsByNewsDataIo();
        var channels = service.GetChannels(data);
        var firstChannel = channels.First();

        var user = Login();
        var result = service.AddBlockedChannel(user.Id, firstChannel);

        Assert.NotNull(result);
    }

    [Fact]
    public void TestAddLikeArticle() 
    {
        var service = new NewsService();
        var likes = new List<Like>();
        var disLikes = new List<DisLike>();
        var data = GetNewsByGoogleTrend();
        var firstArticle = data.First();
        var user = Login();

        var result = service.AddLikeArticle(likes,  disLikes, user.Id, firstArticle.Id);

        Assert.NotNull(likes);
        Assert.Single(result);
    }
    
    [Fact]
    public void TestAddDisLikeArticle() 
    {
        var service = new NewsService();
        var likes = new List<Like>();
        var disLikes = new List<DisLike>();
        var data = GetNewsByGoogleTrend();
        var firstArticle = data.First();
        var user = Login();

        var result = service.AddDisLikeArticle(disLikes, likes, user.Id, firstArticle.Id);

        Assert.NotNull(disLikes);
        Assert.Single(result);
    }

    [Fact]
    public void TestAddDisLikeAndDeleteLikeArticle() 
    {
        var service = new NewsService();
        var likes = new List<Like>();
        var disLikes = new List<DisLike>();
        var data = GetNewsByGoogleTrend();
        var firstArticle = data.First();
        var user = Login();

        likes = service.AddLikeArticle(likes, disLikes, user.Id, firstArticle.Id);
        var result = service.AddDisLikeArticle(disLikes, likes, user.Id, firstArticle.Id);

        Assert.Single(likes);
        likes = service.RemoveLikeArticle(likes, firstArticle.Id);

        Assert.NotNull(disLikes);
        Assert.Single(result);
        Assert.Empty(likes);
    }

    [Fact]
    public void TestAddCommentArticle() 
    {
        var service = new NewsService();
        var data = GetNewsByGoogleTrend();
        var firstArticle = data.First();
        var user = Login();
        var content = "Happy a new day";

        var reuslt = service.AddComment(user.Id, firstArticle.Id, content);

        Assert.NotNull(reuslt);
    }

    [Fact]
    public void TestGetAdArticle()
    {
        var service = new NewsService();
        var config = new Config();
        config.Url = "https://64ae6209c85640541d4cf43d.mockapi.io/AddItem";

        var result = service.GetAdArticles(config.Url);

        Assert.NotNull(result);
        Assert.Equal(10, result.Count);
    }

    [Fact]
    public void TestTopicOfNewsChannels()
    {
        IDataSource datasources = new ApiDataSource();
        var service = new NewsService();
        var config = new Config();
        config.Key = "apikey=" + "pub_2815763c25cffe45251bb8682ef275560ee69";
        config.Language = "&language=" + "vi";
        config.Category = "&category=" + "science,sports,world";
        config.Item = "results";
        config.Url = "https://newsdata.io/api/1/news?"
                             + config.Key
                             + config.Language
                             + config.Category;
        config.MappingTable = new List<MappingTable>()
        {
            new MappingTable("title","Title"),
            new MappingTable("link", "Url") ,
            new MappingTable("description","Description" ),
            new MappingTable("pubDate", "PubDate"),
            new MappingTable("image_url", "ImgUrl"),
            new MappingTable("source_id", "Channel"),
            new MappingTable("category", "Category")
        };
        var data = datasources.GetNews(config);

        var result = service.GetTopicOfNewsChannel(data);

        Assert.NotNull(result);
    }

    [Fact]
    public void TestDefineWeather()
    {
        var weather = new Weather()
        {
            Id = Guid.NewGuid(),
            PubDate = "",
            Description = "",
            HourlyWeather = new List<WeatherInfo>()
        };
        var weatherInfo = new WeatherInfo()
        {
            Id = Guid.NewGuid(),
            Temperature = 23.2,
            WindSpeed = 20,
            AmountOfRain = 30.3,
            Hour = DateTime.Now.Hour,
            WeatherIcon = ""
        };
        weather.HourlyWeather.Add(weatherInfo);

        Assert.NotNull(weather);
        Assert.Single(weather.HourlyWeather);
    }

    [Fact]
    public void TestGetWeatherApi()
    {
        var service = new NewsService();
        var config = new Config();
        config.Headers.RapidApiKey = "63e013be17mshfaa183691e3f9fap12264bjsn8690697c78c9";
        config.Headers.RapidApiHost = "weatherapi-com.p.rapidapi.com";
        config.KeyWork = "q=" + "Ho Chi Minh";
        config.DayNumber = "&day=" + "3";
        config.Language = "&lang=" + "vi";
        config.Location = "location";
        config.Url = "https://weatherapi-com.p.rapidapi.com/forecast.json?" 
            + config.KeyWork
            + config.DayNumber
            + config.Language;
        config.MappingTable = new List<MappingTable>()
        {
            new MappingTable("localtime","PubDate"),
        new MappingTable("name", "Place"),
        };

        var result = service.GetWeatherInfor(config);
        Assert.NotNull(result);
    }
}
