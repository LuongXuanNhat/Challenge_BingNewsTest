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

    private string DownloadXml(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            return client.GetStringAsync(url).Result;
        }
    }

    public List<Article> GetNews(Config config)
    {
        var articles = new List<Article>();

        string xml = DownloadXml(config.Url);
        XDocument document = XDocument.Parse(xml);
        var items = document.Descendants("item");

        foreach (var item in items)
        {
            var article = MapToArticle(item, config);
            articles.Add(article);
        }

        return articles;
    }

    private Article MapToArticle(XElement item, Config config)
    {
        var article = new Article();
        var articleData = new Dictionary<string, string>();
        var mappingTable = config.MappingTable;

        foreach (var property in mappingTable)
        {
            var sourceValue = item.Element(property.SourceProperty)?.Value;
            articleData[property.DestinationProperty] = sourceValue;
        }
        try
        {
            foreach (var property in articleData)
            {
                var propertyInfo = typeof(Article).GetProperty(property.Key);
                var convertedValue = Convert.ChangeType(property.Value, propertyInfo.PropertyType);
                propertyInfo.SetValue(article, convertedValue);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
       

        return article;
    }

}