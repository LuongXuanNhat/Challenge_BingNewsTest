using BingNew.DataAccessLayer.Models;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Linq;
using System.Reflection.PortableExecutable;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using RestSharp;
using System.Threading.Channels;
using System.Numerics;

public class NewsService
{
    private readonly double _viewCoefficient = 0.1;
    private readonly double _likeCoefficient = 0.25;
    private readonly double _disLikeCoefficient = 0.25;
    private readonly double _commentCoefficient = 0.4;

    public NewsService()
    {

    }

    public List<Article> GetDataByChannel(List<Article> data, string channel)
    {
        return data.Where(x => x.Channel.Equals(channel)).ToList() ?? new List<Article>();
    }

    public List<Article> GetTopArticles(List<Article> data, int articleNumber)
    {
        var articles = new List<Article>();
        foreach (var item in data)
        {
            if(GetArticleByDate(item))
            {
                var article = AddData(item);
                articles.Add(article);
            }    
        }
        if(articles.Count < articleNumber) 
        {
            return articles.OrderByDescending(x => x.Score).ToList(); 
        }
        return articles.OrderByDescending(x=>x.Score).Take(articleNumber).ToList();
    }

    private Article AddData(Article item)
    {
        item.ViewNumber = new Random().Next(1, 101);
        item.CommentNumber = new Random().Next(1, 101);
        item.LikeNumber = new Random().Next(1, 101);
        item.DisLikeNumber = new Random().Next(1, 101);
        item.Score = GetScore(item);

        return item;
    }

    private double GetScore(Article item)
    {
        return Math.Round(_viewCoefficient * item.ViewNumber
            + _likeCoefficient * item.LikeNumber
            + _disLikeCoefficient * item.DisLikeNumber
            + _commentCoefficient * item.CommentNumber, 3);
    }

    private bool GetArticleByDate(Article item)
    {
        var getDate = ConvertDateTime(item.PubDate);
        if (getDate.Date == DateTime.Now.Date)
            return true;
        return false;
    }

    private DateTime ConvertDateTime(string pubDate)
    {
        string format = "ddd, dd MMM yyyy HH:mm:ss";
        try
        {
            // Tách thông tin ngày và thời gian từ chuỗi
            string dateTimePart = pubDate.Substring(0, pubDate.IndexOf("GMT") - 1);

            // Chuyển đổi chuỗi thành DateTime
            if (DateTime.TryParseExact(dateTimePart, format, System.Globalization.CultureInfo.InvariantCulture,
                                       System.Globalization.DateTimeStyles.None, out DateTime result))
            { 
                return result;
            }
        } catch (Exception e) {
            Console.WriteLine(e.Message);
        }
       return DateTime.Now;
    }
}