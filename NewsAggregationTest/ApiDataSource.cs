using System.ServiceModel.Syndication;
using System.Xml.Linq;
using System.Xml;
using BingNew.DataAccessLayer.Models;
using Newtonsoft.Json.Linq;

public class ApiDataSource : IDataSource
{
    public ApiDataSource()
    {

    }

    public List<Article> GetNews(Config config)
    {
        var articles = new List<Article>();

        using (HttpClient client = new HttpClient())
        {
            string json = client.GetStringAsync(config.Url).Result;

            JObject jsonObject = JObject.Parse(json);
            JArray newsArray = (JArray)jsonObject["results"] ?? (JArray)jsonObject["articles"];

            foreach (JObject newsItem in newsArray)
            {
                string title = newsItem["title"]?.ToString();
                string link = newsItem["link"]?.ToString() ?? newsItem["url"]?.ToString();
                string description = newsItem["description"]?.ToString();
                string imageUrl = newsItem["image_url"]?.ToString() ?? newsItem["urlToImage"]?.ToString();
                string PubDate = newsItem["pubDate"]?.ToString() ?? newsItem["publishedAt"]?.ToString();
                DateTime pubDate = DateTime.Parse(PubDate);

                articles.Add(new Article(title,link,description,pubDate,imageUrl));
            }
        }
        return articles;
    }
}