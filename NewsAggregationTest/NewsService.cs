using BingNew.DataAccessLayer.Models;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Linq;

public class NewsService
{
    private string _typeRss = "rss";
    private string _urlNewsTrend = "https://trends.google.com/trends/trendingsearches/daily/rss?geo=VN";

    public NewsService()
    {
    }

    public List<Article> GetData(Structure structure)
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

    private List<Article> GetDataFromApi(Structure structure)
    {
        var articles = new List<Article>();
        return articles;
    }

    private List<Article> GetDataFromRss(Structure structure)
    {
        var articles = new List<Article>();

        using (XmlReader reader = XmlReader.Create(structure.Url))
        {
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            XNamespace htNamespace = "https://trends.google.com/trends/trendingsearches/daily";

            foreach (SyndicationItem item in feed.Items)
            {
                var htElements = item.ElementExtensions.Where(e => e.OuterNamespace == htNamespace).ToList();
                string imageUrl = htElements.FirstOrDefault(e => e.OuterName == "picture")?.GetObject<string>();

                articles.Add(new Article()
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
}