using BingNew.DataAccessLayer.Models;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Linq;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;

public class NewsService
{
    private string _typeRss = "rss";
    private string _urlNewsTrend = "https://trends.google.com/trends/trendingsearches/daily/rss?geo=VN";
    private string _typeApi = "api";

    public NewsService()
    {
    }

    public List<BingNew.DataAccessLayer.Models.Article> GetData(Structure structure)
    {
        if (structure.Type.Equals("rss"))
        {
            return GetDataFromRss(structure);
        }  
        else 
        {
            return GetDataFromApi(structure);
        }
    }

    private List<BingNew.DataAccessLayer.Models.Article> GetDataFromApi(Structure structure)
    {
        var articles = new List<BingNew.DataAccessLayer.Models.Article>();

        var newsApiClient = new NewsApiClient("6cbcb9e942954f92a54c65e3714ec500");
        var articlesResponse = newsApiClient.GetEverything(new EverythingRequest
        {
            Q = "Apple",
            SortBy = SortBys.Popularity,
            Language = Languages.EN,
            From = new DateTime(2018, 1, 25)
        });
        if (articlesResponse.Status == Statuses.Ok)
        {
            // total results found
            Console.WriteLine(articlesResponse.TotalResults);
            // here's the first 20
            foreach (var article in articlesResponse.Articles)
            {
                articles.Add(new BingNew.DataAccessLayer.Models.Article()
                {
                    Title = article.Title,
                    Description = article.Description,
                    Link = article.Url,
                    pubDate = new DateTimeOffset(article.PublishedAt.Value) 
                });
            }
        }

        return articles;
    }

    private List<BingNew.DataAccessLayer.Models.Article> GetDataFromRss(Structure structure)
    {
        var articles = new List<BingNew.DataAccessLayer.Models.Article>();

        using (XmlReader reader = XmlReader.Create(structure.Url))
        {
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            XNamespace htNamespace = "https://trends.google.com/trends/trendingsearches/daily";

            foreach (SyndicationItem item in feed.Items)
            {
                var htElements = item.ElementExtensions.Where(e => e.OuterNamespace == htNamespace).ToList();
                string imageUrl = htElements.FirstOrDefault(e => e.OuterName == "picture")?.GetObject<string>();

                articles.Add(new BingNew.DataAccessLayer.Models.Article()
                {
                    Title = item.Title.Text,
                    Link = item.Links[0].Uri.ToString(),
                    Description = item.Summary.Text,
                    PubDate = item.PublishDate.Date,
                    ImageUrl = imageUrl
                });
            }
        }
        return articles;
    }

    public string GetTypeRss()
    {
        return _typeRss;
    }

    public string GetUrlNewsTrend()
    {
        return _urlNewsTrend;
    }

    public string GetTypeApi()
    {
        return _typeApi;
    }
}