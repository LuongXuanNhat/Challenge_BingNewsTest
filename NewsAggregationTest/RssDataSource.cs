using BingNew.DataAccessLayer.Models;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit.Abstractions;

public class RssDataSource : IDataSource
{


    public RssDataSource()
    {

    }

    public List<Article> GetArticles(Config config)
    {
        var articles = new List<Article>();
        
        string xml = DownloadXml(config.Url);
        XDocument document = XDocument.Parse(xml);
        var items = document.Descendants("item");

        foreach (var item in items)
        {
            string title = item.Element("title")?.Value;
            string link = item.Element("link")?.Value;
            string description = item.Element("description")?.Value;
            string pubDate = item.Element("pubDate")?.Value;
            string imgUrl = item.Element("image")?.Value ?? item.Element(config.NameSpace + "picture")?.Value;
            DateTimeOffset PubDate = GetDateTimeOffsetFormat(pubDate);

            var newsItems = item.Elements(config.NameSpace + "news_item");
            if (newsItems != null)
            {
                foreach (var newsItem in newsItems)
                {
                    string newsTitle = newsItem.Element(config.NameSpace + "news_item_title")?.Value;
                    string newsSnippet = newsItem.Element(config.NameSpace + "news_item_snippet")?.Value;
                    string newsUrl = newsItem.Element(config.NameSpace + "news_item_url")?.Value;
                    string newsSource = newsItem.Element(config.NameSpace + "news_item_source")?.Value;
                    description = newsSnippet;
                    
                }
            }
            

            articles.Add(new Article(title, link, description, PubDate, imgUrl));
        }

        return articles;
    }

    // :))
    private DateTimeOffset GetDateTimeOffsetFormat(string? pubDate)
    {
        string format = "ddd, dd MMM yyyy HH:mm:ss";
        int gmtIndex = pubDate.IndexOf("GMT");

        string dateWithoutTimeZone = gmtIndex >= 0 ? pubDate.Substring(0, gmtIndex - 1) : pubDate.Replace(" +0700", ""); ;

        DateTime dateTime = DateTime.ParseExact(dateWithoutTimeZone, format,
            CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

        return DateTimeOffset.Parse(dateWithoutTimeZone);
    }

    private string DownloadXml(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            return client.GetStringAsync(url).Result;
        }
    }
}