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
        string json = DownloadJson(config);
        JObject jsonObject = JObject.Parse(json);

        if ( jsonObject[config.Item] is JArray newsArray)
        {
            foreach (JObject newsItem in newsArray)
            {
                var article = MapToArticle(newsItem, config);
                articles.Add(article);
            }
        }
        return articles;
    }

    private Article MapToArticle(JObject newsItem, Config config)
    {
        var article = new Article();
        var articleData = new Dictionary<string, string>();
        var mappingTable = config.MappingTable;

        foreach (var property in mappingTable)
        {
            var sourceValue = newsItem[property.SourceProperty]?.ToString();
            if (sourceValue != null)
            {
                articleData[property.DestinationProperty] = sourceValue;
            }
        }
        try
        {
            foreach (var property in articleData)
            {
                var propertyInfo = typeof(Article).GetProperty(property.Key);
                if (propertyInfo != null)
                {
                    var convertedValue = Convert.ChangeType(property.Value, propertyInfo.PropertyType);
                    propertyInfo.SetValue(article, convertedValue);
                }
                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        return article;
    }

    private string DownloadJson(Config config)
    {

        using (HttpClient client = new HttpClient())
        {
            return client.GetStringAsync(config.Url).Result;
        }
    }
}