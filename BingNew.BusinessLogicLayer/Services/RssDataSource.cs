using BingNew.BusinessLogicLayer.Services;
using BingNew.DataAccessLayer.Models;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        var items = document.Descendants(config.Item);

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
            
            if (sourceValue != null)
            {
                articleData[property.DestinationProperty] = sourceValue;
            }
            else 
            {
                var newItem = config.Namespace + property.SourceProperty;
                var sourceElement = item.Element(newItem);

                if (newItem != null && sourceElement != null)
                {
                    articleData[property.DestinationProperty] = sourceElement.Value;
                }
            }
            
        }


        //foreach (var dictionnary in articleData)
        //{
        //    if (dictionnary.Value == null)
        //    {

        //        articleData[dictionnary.Key] = item.Element(config.Namespace + property.SourceProperty)?.Value;
        //    }
        //    if (config.Channel != null && articleData[config.Channel] == null) {
        //    var property = mappingTable.Where(x => x.DestinationProperty.Equals(config.Channel)).FirstOrDefault();
        //    articleData[property.DestinationProperty] = item.Element(config.Namespace + property.SourceProperty)?.Value;
        //}

        try
        {
            foreach (var property in articleData)
            {
                var propertyInfo = typeof(Article).GetProperty(property.Key);
                if (propertyInfo != null && propertyInfo.PropertyType != null)
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

}