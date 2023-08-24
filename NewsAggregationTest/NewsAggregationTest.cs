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
        structure.Url = "https://bloomberg-market-and-financial-news.p.rapidapi.com/market/auto-complete?query=%3CREQUIRED%3E";
        structure.Headers = new RequestHeaders
        {
            RapidApiKey = "a85a42c628msh3c18900ec8b7b37p1e83c3jsn7e19527b9aa4",
            RapidApiHost = "bloomberg-market-and-financial-news.p.rapidapi.com"
        };

        var result = service.GetArticles(structure);
        Assert.NotNull(result);
    }
}