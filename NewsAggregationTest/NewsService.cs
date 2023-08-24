using BingNew.DataAccessLayer.Models;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Linq;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using System.Reflection.PortableExecutable;
using Newtonsoft.Json.Linq;

public class NewsService
{
    private string _typeRss = "rss";
    private string _urlNewsTrend = "https://trends.google.com/trends/trendingsearches/daily/rss?geo=VN";
    private string _typeApi = "api";

    public NewsService()
    {
    }

    public List<BingNew.DataAccessLayer.Models.Article> GetArticles(Structure structure)
    {
        if (structure.Type.Equals("rss"))
        {
            return GetDataFromRss(structure);
        }  
        else if (structure.Type.Equals("api"))
        {
            return GetDataFromApi(structure);
        } else
        {
            return GetDataFromApiNewsDataIo(structure);
        }
    }

    private List<BingNew.DataAccessLayer.Models.Article> GetDataFromApiNewsDataIo(Structure structure)
    {
        var articles = new List<BingNew.DataAccessLayer.Models.Article>();

        using (HttpClient client = new HttpClient())
        {
            string json = client.GetStringAsync(structure.Url).Result;

            JObject jsonObject = JObject.Parse(json);
            JArray newsArray = (JArray)jsonObject["results"];

            foreach (JObject newsItem in newsArray)
            {
                string title = newsItem["title"].ToString();
                string link = newsItem["link"].ToString();
                string description = newsItem["description"].ToString();
                string imageUrl = newsItem["image_url"].ToString() ?? string.Empty;
                DateTime pubDate = DateTime.Parse(newsItem["pubDate"].ToString());

                articles.Add(new BingNew.DataAccessLayer.Models.Article()
                {
                    Title = title,
                    Link = link,
                    Description = description,
                    PubDate = new DateTimeOffset( pubDate),
                    ImageUrl = imageUrl
                });
            }
        }
        return articles;
    }

    private List<BingNew.DataAccessLayer.Models.Article> GetDataFromApi(Structure structure)
    {
        var articles = new List<BingNew.DataAccessLayer.Models.Article>();

        var newsApiClient = new NewsApiClient(structure.Key);
        var articlesResponse = newsApiClient.GetEverything(new EverythingRequest
        {
            Q = "news",
            SortBy = SortBys.PublishedAt,
            Language = Languages.EN,
            From = new DateTime(2023, 8, 24)
        }) ;
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
                    PubDate = new DateTimeOffset(article.PublishedAt.GetValueOrDefault()) 
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