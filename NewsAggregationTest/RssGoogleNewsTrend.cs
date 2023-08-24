using BingNew.DataAccessLayer.Models;
using Newtonsoft.Json.Linq;
using System.ServiceModel.Syndication;
using System.Xml.Linq;
using System.Xml;

public class RssGoogleNewsTrend : ITypeRssSource
{
    public List<Article> GetArticles(Config config)
    {
        var articles = new List<Article>();

        using (XmlReader reader = XmlReader.Create(config.Url))
        {
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            XNamespace htNamespace = "https://trends.google.com/trends/trendingsearches/daily";

            foreach (SyndicationItem item in feed.Items)
            {
                var htElements = item.ElementExtensions.Where(e => e.OuterNamespace == htNamespace).ToList();
                string imageUrl = htElements.FirstOrDefault(e => e.OuterName == "picture")?.GetObject<string>();
                string channelTitle = htElements.FirstOrDefault(e => e.OuterName == "picture_source")?.GetObject<string>();

                articles.Add(new Article()
                {
                    Title = item.Title.Text,
                    Link = item.Links[0].Uri.ToString(),
                    Description = item.Summary.Text,
                    PubDate = item.PublishDate.Date,
                    ImageUrl = imageUrl,
                    Channel = channelTitle
                });
            }
        }
        return articles;
    }
}