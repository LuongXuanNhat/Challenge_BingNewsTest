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
    public void TestGetNews()
    {
        var service = new NewsService();
        var structure = new Structure();
        structure.Type = service.GetTypeApi();
        structure.Key = "6cbcb9e942954f92a54c65e3714ec500";

        var result = service.GetArticles(structure);
        Assert.NotNull(result);
    }
}