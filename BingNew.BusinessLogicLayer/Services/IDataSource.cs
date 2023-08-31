using BingNew.DataAccessLayer.Models;

namespace BingNew.BusinessLogicLayer.Services;
public interface IDataSource
{
    public List<Article> GetNews(Config config);
}