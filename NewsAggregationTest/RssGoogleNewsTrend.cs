using BingNew.DataAccessLayer.Models;
using Newtonsoft.Json.Linq;

public class RssGoogleNewsTrend : ITypeRssSource
{
    public List<Article> GetArticles(Config config)
    {
        config.Type.Equals("rssType");

        var articles = new List<Article>();


        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(config.Url),
            Headers =
            {
                { "X-BingApis-SDK", "true" },
                { "X-RapidAPI-Key", config.Headers.RapidApiKey },
                { "X-RapidAPI-Host", config.Headers.RapidApiHost },
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
}