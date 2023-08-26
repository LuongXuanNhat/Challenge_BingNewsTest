﻿using BingNew.DataAccessLayer.Models;
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
            articleData[property.DestinationProperty] = sourceValue;

            if (articleData[property.DestinationProperty] == null)
            {
                articleData[property.DestinationProperty] = item.Element(config.Namespace + property.SourceProperty)?.Value;
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