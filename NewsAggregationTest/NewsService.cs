using BingNew.DataAccessLayer.Models;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Linq;
using System.Reflection.PortableExecutable;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using RestSharp;
using System.Threading.Channels;

public class NewsService
{
    private string _typeRss = "rss";
    private string _urlNewsTrend = "https://trends.google.com/trends/trendingsearches/daily/rss?geo=VN";
    private string _typeApi = "api";
    private string _rssTypeOfGoogleNewsTrend = "RSS_GOOGLE_NEWS_TREND";
    private string _rssTypeOfTuoiTreNews = "RSS_TUOI_TRE_NEWS";
    private string _apiTypeOfNewsDataIo = "RSS_NEWS_DATA_IO";

    public NewsService()
    {

    }
    private List<Article> GetDataFromApiNewsDataIo(Config structure)
    {
        var articles = new List<Article>();

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

                articles.Add(new Article()
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

    private List<Article> GetDataFromApi(Config structure)
    {
        var articles = new List<Article>();
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

                articles.Add(new Article()
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

    private List<Article> GetDataFromRss(Config structure)
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
                string channelTitle = htElements.FirstOrDefault(e => e.OuterName == "picture_source")?.GetObject<string>();

                articles.Add(new Article()
                {
                    Title = item.Title.Text,
                    Link = item.Links[0].Uri.ToString(),
                    Description = item.Summary.Text,
                    PubDate = item.PublishDate.Date,
                    ImageUrl = imageUrl,
                    Channel = channelTitle
                }) ;
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

    public List<Article> GetArticleByChannel(List<Article> articles, string channel)
    {
        return articles.Where(x=>x.Channel.Equals(channel)).ToList();
    }

    public string GetTypeRssGoogleTrend()
    {
        return _rssTypeOfGoogleNewsTrend;
    }

    public string GetTypeRssTuoiTreNews()
    {
        return _rssTypeOfTuoiTreNews;
    }

    public string GetTypeApiNewDataIo()
    {
        return _apiTypeOfNewsDataIo;
    }
}