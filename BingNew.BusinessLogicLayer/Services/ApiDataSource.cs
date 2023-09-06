using System.ServiceModel.Syndication;
using System.Xml.Linq;
using System.Xml;
using BingNew.DataAccessLayer.Models;
using Newtonsoft.Json.Linq;
using BingNew.BusinessLogicLayer.Services;

public class ApiDataSource : IDataSource
{
    public ApiDataSource()
    {

    }

    public List<Article> GetNews(Config config)
    {
        var articles = new List<Article>();
        string json = DownloadJson(config.Url);
        JObject jsonObject = JObject.Parse(json);

        //if ( jsonObject[config.Item] is JArray newsArray)
        //{
        //    foreach (JObject newsItem in newsArray)
        //    {
        //        var article = MapToArticle(newsItem, config);
        //        articles.Add(article);
        //    }
       // }
        return articles;
    }

    private Article MapToArticle(JObject item, List<MappingTable> mapping)
    {
        var article = new Article();
        foreach (var obj in mapping)
        {
            try
            {
                var sourceValue = item[obj.SouPropertyPath]?.ToString();
                obj.SouValue = sourceValue ?? string.Empty;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            var propertyInfo = typeof(Article).GetProperty(obj.DesProperty);
            if (propertyInfo != null && propertyInfo.PropertyType != null)
            {
                if (obj.SouDatatype == "string" && obj.DesDatatype == "DateTime")
                {
                    string dateString = obj.SouValue;
                    dateString = dateString.Replace(" GMT+7", "");
                    var convertedValue = DateTime.Parse(dateString);
                    propertyInfo.SetValue(article, convertedValue);
                }
                else
                {
                    var convertedValue = Convert.ChangeType(obj.SouValue, propertyInfo.PropertyType);
                    propertyInfo.SetValue(article, convertedValue);
                }
            }
        }
        return article;
    }

    private string DownloadJson(string Url)
    {
        using (HttpClient client = new HttpClient())
        {
            return client.GetStringAsync(Url).Result;
        }
    }

    public string GetNews(string Url)
    {
        return DownloadJson(Url);
    }

    public List<Article> ConvertDataToArticles(Config config, List<MappingTable> mapping)
    {
        var articles = new List<Article>();
        JObject jsonObject = JObject.Parse(config.Data);

        if (jsonObject[config.Item] is JArray newsArray)
        {
            foreach (JObject newsItem in newsArray)
            {
                var article = MapToArticle(newsItem, mapping);
                articles.Add(article);
            }
        }
        return articles;
    }
}