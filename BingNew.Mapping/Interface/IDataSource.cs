namespace BingNew.Mapping.Interface;
public interface IDataSource
{
    public Tuple<bool, IEnumerable<object>, string> MultipleMapping(List<CustomConfig> customConfigs);
    public string GetData(Config config);
}
