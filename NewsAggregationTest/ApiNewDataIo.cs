using BingNew.DataAccessLayer.Models;
using Newtonsoft.Json.Linq;

public class ApiNewDataIo : ITypeRssSource
{

    public List<Article> GetArticles(Config config)
    {
        var articles = new List<Article>();

        using (HttpClient client = new HttpClient())
        {
            string json = client.GetStringAsync(config.Url).Result;

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
                    PubDate = new DateTimeOffset(pubDate),
                    ImageUrl = imageUrl
                });
            }
        }
        return articles;
    }
}