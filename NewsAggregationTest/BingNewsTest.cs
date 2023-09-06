
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
        config.Url = "https://newsdata.io/api/1/news?" + config.Key + config.Language;
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

        var mappingConfig = _newsService.CreateMapping(_dataSample.MappingData_RssTuoiTreNews());
        var result = _dataSource.ConvertDataToArticles(_config, mappingConfig);

        Assert.NotNull(result);
    }

    [Fact]
    public void ConvertData2ToArticlesNotNull()
    {
        _dataSource = new RssDataSource();
        _config.Data = _dataSource.GetNews("https://trends.google.com.vn/trends/trendingsearches/daily/rss?geo=VN");
        _config.Item = "item";

        var mappingConfig = _newsService.CreateMapping(_dataSample.MappingData_GgTrends());
        var result = _dataSource.ConvertDataToArticles(_config, mappingConfig);

        Assert.NotNull(result);
    }

    [Fact]
    public void ConvertData3ToArticlesNotNull()
    {
        _dataSource = new ApiDataSource();
        _config.Key = "apikey=" + "pub_2815763c25cffe45251bb8682ef275560ee69";
        _config.Language = "&language=" + "vi";
        _config.Category = "&category=" + "business,entertainment";
        _config.Url = "https://newsdata.io/api/1/news?" + _config.Key + _config.Language + _config.Category;
        _config.Data = _dataSource.GetNews(_config.Url);
        _config.Item = "results";

        var mappingConfig = _newsService.CreateMapping(_dataSample.MappingData_NewsDataIo());
        var result = _dataSource.ConvertDataToArticles(_config, mappingConfig);

        Assert.NotNull(result);
    }

    

}
