namespace BingNew.Mapping.Interface;
public interface IDataSource
{
 ////   public List<T> ListMapping<T>(Config config, List<CustomConfig> mapping) where T : new();
    ////public string GetNews(string Url);
    public Tuple<bool, IEnumerable<object>, string> MultipleMapping(List<CustomConfig> customConfigs);
 ////   public T SingleMapping<T>(string data, List<CustomConfig> mapping) where T : new();
    public string GetData(Config config);
}