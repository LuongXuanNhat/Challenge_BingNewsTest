public class NewsAggregationTest
{
    [Fact]
    public void TestNewsService()
    {
        var service = new NewsService();
        Assert.NotNull(service);
    }

    [Fact] 
    public void TestNewsTrend() {
        var service = new NewsService();
        var structure = new Structure();
        structure.Type = service.GetTypeRss();
        structure.Url = service.GetUrlNewsTrend();
        var result = service.GetData(structure);

        Assert.NotNull(result);
    }
}