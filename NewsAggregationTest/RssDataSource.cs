using BingNew.DataAccessLayer.Models;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

public class RssDataSource : IDataSource
{
    private Dictionary<string, ITypeRssSource> _rssDataSource;
    private NewsService _newsService;

    public RssDataSource()
    {
        _newsService = new NewsService();
        _rssDataSource = new Dictionary<string, ITypeRssSource>();
        _rssDataSource[_newsService.GetTypeRssGoogleTrend()] = new RssGoogleNewsTrend();
        _rssDataSource[_newsService.GetTypeRssTuoiTreNews()] = new RssTuoiTreNews();
    }

    public List<Article> GetNews(Config config)
    {
        return _rssDataSource[config.Type].GetArticles(config); 
    }
}