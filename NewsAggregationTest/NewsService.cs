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

    public NewsService()
    {

    }

    //private List<Article> GetDataFromApi(Config structure)
    //{
    //    var articles = new List<Article>();
    //    var client = new HttpClient();
    //    var request = new HttpRequestMessage
    //    {
    //        Method = HttpMethod.Get,
    //        RequestUri = new Uri(structure.Url),
    //        Headers =
    //        {
    //            { "X-BingApis-SDK", "true" },
    //            { "X-RapidAPI-Key", structure.Headers.RapidApiKey },
    //            { "X-RapidAPI-Host", structure.Headers.RapidApiHost },
    //        },
    //    };
    //    using (var response = client.Send(request))
    //    {
    //        response.EnsureSuccessStatusCode();
    //        var body = response.Content.ReadAsStringAsync().Result;
    //        JObject jsonObject = JObject.Parse(body);
    //        JArray newsArray = (JArray)jsonObject["value"];

    //        foreach (JObject newsItem in newsArray)
    //        {
    //            string title = newsItem["name"].ToString();
    //            string link = newsItem["url"].ToString();
    //            string description = newsItem["description"].ToString();
    //            string imageUrl = newsItem["image"]?["thumbnail"]?["contentUrl"].ToString();
    //            DateTime pubDate = DateTime.Parse(newsItem["datePublished"].ToString());

    //            articles.Add(new Article()
    //            {
    //                Title = title,
    //                Url = link,
    //                Description = description,
    //                PubDate = pubDate,
    //                ImgUrl = imageUrl
    //            });
    //        }
    //    }
    //    return articles;
    //}
}