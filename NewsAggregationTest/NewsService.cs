using BingNew.DataAccessLayer.Models;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Linq;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using System.Reflection.PortableExecutable;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

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


        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(structure.Url),
            Headers =
            {
                { "X-BingApis-SDK", "true" },
                { "X-RapidAPI-Key", structure.Headers.RapidApiKey },
                { "X-RapidAPI-Host", structure.Headers.RapidApiHost },
            },
        };
        using (var response = client.Send(request))
        {
            response.EnsureSuccessStatusCode();
            var body = response.Content.ReadAsStringAsync().Result;
            JObject jsonObject = JObject.Parse(body);
            JArray newsArray = (JArray)jsonObject["value"];

            foreach (JObject newsItem in newsArray)
            {
                string title = newsItem["name"].ToString();
                string link = newsItem["url"].ToString();
                string description = newsItem["description"].ToString();
                string imageUrl = newsItem["image"]?["thumbnail"]?["contentUrl"].ToString();
                DateTime pubDate = DateTime.Parse(newsItem["datePublished"].ToString());

                articles.Add(new BingNew.DataAccessLayer.Models.Article()
                {
                    Title = title,
                    Link = link,
                    Description = description,
                    PubDate = pubDate,
                    ImageUrl = imageUrl
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