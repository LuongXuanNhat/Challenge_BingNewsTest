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

    public List<Article> GetDataByChannel(List<Article> data, string channel)
    {
        return data.Where(x => x.Channel.Equals(channel)).ToList() ?? new List<Article>();
    }
}