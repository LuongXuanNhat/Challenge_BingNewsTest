using System.ServiceModel.Syndication;
using System.Xml.Linq;
using System.Xml;
using BingNew.DataAccessLayer.Models;

public class ApiDataSource : IDataSource
{

    private Dictionary<string, ITypeRssSource> _apiDataSource;
    private NewsService _newsService;

    public ApiDataSource()
    {
        _newsService = new NewsService();
        _apiDataSource = new Dictionary<string, ITypeRssSource>();
        _apiDataSource[_newsService.GetTypeApiNewDataIo()] = new ApiNewDataIo();
    }

    public List<Article> GetNews(Config config)
    {
        return _apiDataSource[config.Type].GetArticles(config);
    }

}