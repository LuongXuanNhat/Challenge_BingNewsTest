
using BingNew.BusinessLogicLayer.Services;
using BingNew.DataAccessLayer.Models;
using NewsAggregationTest.TestData;

public class BingNewsTest
{
    private IDataSource _dataSource;
    private NewsService _newsService;
    private DataSample _dataSample;
    private Config _config;
    public BingNewsTest()
    {
        _dataSample = new DataSample();
        _newsService = new NewsService();
        _config = new Config();
    }

    private User Login()
    {
        return new User("1", "luongxuannhat", "email@gmail.com");
    }
    private string GetNewsByNewsDataIo()
    {
        var config = new Config();
        config.Key = "apikey=" + "pub_2815763c25cffe45251bb8682ef275560ee69";
        config.Language = "&language=" + "vi";
        config.Item = "results";
        config.Url = "https://newsdata.io/api/1/news?"
                             + config.Key
                             + config.Language;
        _dataSource = new ApiDataSource();

        return _dataSource.GetNews(config.Url);
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
        //mappingConfig.MappingTable = new List<MappingTable>()
        //{
        //    new MappingTable("title","Title"),
        //    new MappingTable("link", "Url") ,
        //    new MappingTable("description","Description" ),
        //    new MappingTable("pubDate", "PubDate"),
        //    new MappingTable("picture", "ImgUrl"),
        //    new MappingTable("picture_source", "Channel")

        //};

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
        //mappingConfig.MappingTable = new List<MappingTable>()
        //{
        //    new MappingTable("title","Title"),
        //    new MappingTable("link", "Url") ,
        //    new MappingTable("description","Description" ),
        //    new MappingTable("pubDate", "PubDate"),
        //    new MappingTable("image", "ImgUrl")
        //};

        return dataSource.GetNews(config);
    }

    [Fact]
    public void CreateNewsServiceNotNull()
    {
        var service = new NewsService();
        Assert.NotNull(service);
    }
    
    [Fact]
    public void GetNewsFromRssTuoiTreNotNull()
    {
        _dataSource = new RssDataSource();
        var result = _dataSource.GetNews("https://tuoitre.vn/rss/tin-moi-nhat.rss");
        Assert.NotNull(result);
    }

    [Fact]
    public void GetNewsFromRssGoogleTrendNotNull()
    {
        _dataSource = new RssDataSource();
        var result = _dataSource.GetNews("https://trends.google.com.vn/trends/trendingsearches/daily/rss?geo=VN");
        Assert.NotNull(result);
    }

    [Fact]
    public void GetNewsFromApiNewsDataIoNotNull()
    {
        var config = new Config();
        config.Key = "apikey=" + "pub_2815763c25cffe45251bb8682ef275560ee69";
        config.Language = "&language=" + "vi";
        config.Item = "results";
        config.Url = "https://newsdata.io/api/1/news?"
                             + config.Key
                             + config.Language;
        _dataSource = new ApiDataSource();

        var result = _dataSource.GetNews(config.Url);

        Assert.NotNull(result);
    }

    [Fact]
    public void ConvertData1ToArticlesNotNull()
    {
        _dataSource = new RssDataSource();
        _config.Data = _dataSource.GetNews("https://tuoitre.vn/rss/tin-moi-nhat.rss");
        _config.Item = "item";
        _config.Channel = "Tuoi Tre News";

        var mappingConfig = _newsService.CreateMapping(_dataSample.MappingData_1());
        var result = _dataSource.ConvertDataToArticles(_config, mappingConfig);

        Assert.NotNull(result);
    }

    [Fact]
    public void ConvertData2ToArticlesNotNull()
    {
        _dataSource = new RssDataSource();
        _config.Data = _dataSource.GetNews("https://trends.google.com.vn/trends/trendingsearches/daily/rss?geo=VN");
        _config.Item = "item";

        var mappingConfig = _newsService.CreateMapping(_dataSample.MappingData_1());
        var result = _dataSource.ConvertDataToArticles(_config, mappingConfig);

        Assert.NotNull(result);
    }

    //[Fact]
    //public void GetNewsByChannelFromNewsDataIo_NotNull()
    //{
    //    var service = new NewsService();
    //    var data = GetNewsByNewsDataIo();
    //    var channel = "kenh14";

    //    var result = service.GetDataByChannel(data, channel);

    //    Assert.NotNull(result);
    //}

    //[Fact]
    //public void GetNewsByChannelFromGoogleTrend()
    //{
    //    var service = new NewsService();
    //    var data = GetNewsByGoogleTrend();
    //    var channel = "Vietnamnet.vn";

    //    var result = service.GetDataByChannel(data, channel);

    //    Assert.NotNull(result);
    //}

    //[Fact]
    //public void GetNewsByCategoryFromNewsDataIo()
    //{
    //    IDataSource datasources = new ApiDataSource();
    //    var service = new NewsService();
    //    var mappingConfig = new Config();
    //    mappingConfig.Key = "apikey=" + "pub_2815763c25cffe45251bb8682ef275560ee69";
    //    mappingConfig.Language = "&language=" + "vi";
    //    mappingConfig.Category = "&category=" + "business,entertainment";
    //    mappingConfig.Item = "results";
    //    mappingConfig.Url = "https://newsdata.io/api/1/news?"
    //                         + mappingConfig.Key
    //                         + mappingConfig.Language
    //                         + mappingConfig.Category;
    //    mappingConfig.MappingTable = new List<MappingTable>()
    //    {
    //        new MappingTable("title","Title"),
    //        new MappingTable("link", "Url") ,
    //        new MappingTable("description","Description" ),
    //        new MappingTable("pubDate", "PubDate"),
    //        new MappingTable("image_url", "ImgUrl"),
    //        new MappingTable("source_id", "Channel"),
    //        new MappingTable("category", "Category")
    //    };
    //    var result = datasources.GetNews(mappingConfig);

    //    Assert.NotNull(result);
    //}

    //[Fact]
    //public void TestGetChannelFromGoogleTrend()
    //{
    //    var service = new NewsService();
    //    var data = GetNewsByGoogleTrend();
    //    var channels = service.GetChannels(data);

    //    Assert.NotNull(channels);
    //}


    //[Fact]
    //public void TestGetChannelFromNewsDataIo()
    //{
    //    var service = new NewsService();
    //    var data = GetNewsByNewsDataIo();
    //    var channels = service.GetChannels(data);

    //    Assert.NotNull(channels);
    //}

    //[Fact]
    //public void TestTrendingStoriesOfTuoiTreNews()
    //{
    //    var service = new NewsService();
    //    var data = GetNewsByTuoiTreNews();
    //    var articleNumber = 10;
    //    var trendingNews = service.GetTopArticles(data, articleNumber);

    //    Assert.NotNull(trendingNews);
    //}

    //[Fact]
    //public void TestTrendingStoriesOfNewsDataIo()
    //{
    //    var service = new NewsService();
    //    var data = GetNewsByNewsDataIo();
    //    var articleNumber = 7;
    //    var trendingNews = service.GetTopArticles(data, articleNumber);

    //    Assert.NotNull(trendingNews);
    //}

    //[Fact]
    //public void TestAddFavoriteChannelToFollowList()
    //{
    //    var service = new NewsService();
    //    var data = GetNewsByGoogleTrend();
    //    var channels = service.GetChannels(data);
    //    var firstChannel = channels.First();

    //    var user = new User("1", "luongxuannhat", "email@gmail.com");
    //    var result = service.AddFavoriteChannel(user.Id, firstChannel);

    //    Assert.NotNull(result);
    //}

    //[Fact]
    //public void TestRemoveFavoriteChannelFromFollowList()
    //{
    //    var service = new NewsService();
    //    var data = GetNewsByGoogleTrend();
    //    var channels = service.GetChannels(data);
    //    var firstChannel = channels.First();
    //    var user = Login();
    //    var result = service.AddFavoriteChannel(user.Id, firstChannel);
    //    result = service.RemoveFavoriteChannel(user.Id, firstChannel, result);

    //    Assert.Empty(result);
    //}



    //[Fact]
    //public void TestAddHateChannelToBlockedList()
    //{
    //    var service = new NewsService();
    //    var data = GetNewsByNewsDataIo();
    //    var channels = service.GetChannels(data);
    //    var firstChannel = channels.First();

    //    var user = Login();
    //    var result = service.AddBlockedChannel(user.Id, firstChannel);

    //    Assert.NotNull(result);
    //}

    //[Fact]
    //public void TestAddLikeArticle() 
    //{
    //    var service = new NewsService();
    //    var likes = new List<Like>();
    //    var disLikes = new List<DisLike>();
    //    var data = GetNewsByGoogleTrend();
    //    var firstArticle = data.First();
    //    var user = Login();

    //    var result = service.AddLikeArticle(likes,  disLikes, user.Id, firstArticle.Id);

    //    Assert.NotNull(likes);
    //    Assert.Single(result);
    //}

    //[Fact]
    //public void TestAddDisLikeArticle() 
    //{
    //    var service = new NewsService();
    //    var likes = new List<Like>();
    //    var disLikes = new List<DisLike>();
    //    var data = GetNewsByGoogleTrend();
    //    var firstArticle = data.First();
    //    var user = Login();

    //    var result = service.AddDisLikeArticle(disLikes, likes, user.Id, firstArticle.Id);

    //    Assert.NotNull(disLikes);
    //    Assert.Single(result);
    //}

    //[Fact]
    //public void TestAddDisLikeAndDeleteLikeArticle() 
    //{
    //    var service = new NewsService();
    //    var likes = new List<Like>();
    //    var disLikes = new List<DisLike>();
    //    var data = GetNewsByGoogleTrend();
    //    var firstArticle = data.First();
    //    var user = Login();

    //    likes = service.AddLikeArticle(likes, disLikes, user.Id, firstArticle.Id);
    //    var result = service.AddDisLikeArticle(disLikes, likes, user.Id, firstArticle.Id);

    //    Assert.Single(likes);
    //    likes = service.RemoveLikeArticle(likes, firstArticle.Id);

    //    Assert.NotNull(disLikes);
    //    Assert.Single(result);
    //    Assert.Empty(likes);
    //}

    //[Fact]
    //public void TestAddCommentArticle() 
    //{
    //    var service = new NewsService();
    //    var data = GetNewsByGoogleTrend();
    //    var firstArticle = data.First();
    //    var user = Login();
    //    var content = "Happy a new day";

    //    var reuslt = service.AddComment(user.Id, firstArticle.Id, content);

    //    Assert.NotNull(reuslt);
    //}

    //[Fact]
    //public void TestGetAdArticle()
    //{
    //    var service = new NewsService();
    //    var mappingConfig = new Config();
    //    mappingConfig.Url = "https://64ae6209c85640541d4cf43d.mockapi.io/AddItem";

    //    var result = service.GetAdArticles(mappingConfig.Url);

    //    Assert.NotNull(result);
    //    Assert.Equal(10, result.Count);
    //}

    //[Fact]
    //public void TestTopicOfNewsChannels()
    //{
    //    IDataSource datasources = new ApiDataSource();
    //    var service = new NewsService();
    //    var mappingConfig = new Config();
    //    mappingConfig.Key = "apikey=" + "pub_2815763c25cffe45251bb8682ef275560ee69";
    //    mappingConfig.Language = "&language=" + "vi";
    //    mappingConfig.Category = "&category=" + "science,sports,world";
    //    mappingConfig.Item = "results";
    //    mappingConfig.Url = "https://newsdata.io/api/1/news?"
    //                         + mappingConfig.Key
    //                         + mappingConfig.Language
    //                         + mappingConfig.Category;
    //    mappingConfig.MappingTable = new List<MappingTable>()
    //    {
    //        new MappingTable("title","Title"),
    //        new MappingTable("link", "Url") ,
    //        new MappingTable("description","Description" ),
    //        new MappingTable("pubDate", "PubDate"),
    //        new MappingTable("image_url", "ImgUrl"),
    //        new MappingTable("source_id", "Channel"),
    //        new MappingTable("category", "Category")
    //    };
    //    var data = datasources.GetNews(mappingConfig);

    //    var result = service.GetTopicOfNewsChannel(data);

    //    Assert.NotNull(result);
    //}

    //[Fact]
    //public void TestDefineWeather()
    //{
    //    var weather = new Weather()
    //    {
    //        Id = Guid.NewGuid(),
    //        PubDate = "",
    //        Description = "",
    //        HourlyWeather = new List<WeatherInfo>()
    //    };
    //    var weatherInfo = new WeatherInfo()
    //    {
    //        Id = Guid.NewGuid(),
    //        Temperature = 23.2,
    //        WindSpeed = 20,
    //        AmountOfRain = 30.3,
    //        Hour = DateTime.Now.Hour,
    //        WeatherIcon = ""
    //    };
    //    weather.HourlyWeather.Add(weatherInfo);

    //    Assert.NotNull(weather);
    //    Assert.Single(weather.HourlyWeather);
    //}

    //[Fact]
    //public void TestGetWeatherApi()
    //{
    //    var service = new NewsService();
    //    var mappingConfig = new Config();
    //    mappingConfig.Headers.RapidApiKey = "63e013be17mshfaa183691e3f9fap12264bjsn8690697c78c9";
    //    mappingConfig.Headers.RapidApiHost = "weatherapi-com.p.rapidapi.com";
    //    mappingConfig.KeyWork = "q=" + "Ho Chi Minh";
    //    mappingConfig.DayNumber = "&day=" + "3";
    //    mappingConfig.Language = "&lang=" + "vi";
    //    mappingConfig.Location = "location";
    //    mappingConfig.Url = "https://weatherapi-com.p.rapidapi.com/forecast.json?" 
    //        + mappingConfig.KeyWork
    //        + mappingConfig.DayNumber
    //        + mappingConfig.Language;
    //    mappingConfig.MappingTable = new List<MappingTable>()
    //    {
    //        new MappingTable("localtime","PubDate"),
    //    new MappingTable("name", "Place"),
    //    };

    //    var result = service.GetWeatherInfor(mappingConfig);
    //    Assert.NotNull(result);
    //}
}
