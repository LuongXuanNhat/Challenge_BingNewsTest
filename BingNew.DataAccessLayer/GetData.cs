using BingNew.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace BingNew.DataAccessLayer
{
    public class GetData
    {

        public string DownloadRssContent(string url)
        {
            using (var client = new WebClient())
            {
                return client.DownloadString(url);
            }
        }

        //public List<Article> ParseRssContent(string rssContent)
        //{
        //    List<Article> articles = new List<Article>();

        //    XmlDocument xmlDoc = new XmlDocument();
        //    xmlDoc.LoadXml(rssContent);

        //    XmlNodeList itemNodes = xmlDoc.SelectNodes("//item");
        //    foreach (XmlNode itemNode in itemNodes)
        //    {
        //        string? id = itemNode.SelectSingleNode("guid")?.InnerText.Trim();
        //        string? title = itemNode.SelectSingleNode("title")?.InnerText.Trim();
        //        string? link = itemNode.SelectSingleNode("link")?.InnerText.Trim();
        //        string? description = itemNode.SelectSingleNode("description")?.InnerText.Trim();
        //        string? pubDate = itemNode.SelectSingleNode("pubDate")?.InnerText.Trim();
        //        string? urlImage = GetUrlImage(description);

        //        DateTimeOffset date = ParseDateString(pubDate);

        //        articles.Add(new Article(id, title, date, link, description, urlImage));
        //    }

        //    return articles;
        //}

        private string GetUrlImage(string description)
        {
            string pattern = @"src=""(.*?)""";
            Match match = Regex.Match(description, pattern);

            if (match.Success)
            {
                string imageUrl = match.Groups[1].Value;
                return imageUrl;
            }
            else
            {
                return "";
            }
        }

        public DateTimeOffset ParseDateString(string dateString)
        {
            DateTimeOffset result;
            string format = "ddd, dd MMM yyyy HH:mm:ss zzz";

            if (DateTimeOffset.TryParseExact(dateString, format,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out result))
            {
                return result;
            }
            throw new FormatException("Invalid date format");
        }
    }
}
