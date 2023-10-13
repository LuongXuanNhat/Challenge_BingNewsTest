namespace BingNew.Mapping.Interface;
public interface IDataSource
{
    public List<T> ConvertDataToArticles<T>(Config config, List<CustomConfig> mapping) where T : new();
    public string GetNews(string Url);
    public T ConvertDataToType<T>(string data, List<CustomConfig> mapping) where T : new();
    public string GetWeatherInfor(Config config);
}