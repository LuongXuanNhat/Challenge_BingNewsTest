using BingNew.DataAccessLayer.Models;

namespace BingNew.BusinessLogicLayer.Services;
public interface IDataSource
{
    public List<Article> ConvertDataToArticles(Config config, List<MappingTable> mapping);
    public List<Article> GetNews(Config config);
    public string GetNews(string Url);
}