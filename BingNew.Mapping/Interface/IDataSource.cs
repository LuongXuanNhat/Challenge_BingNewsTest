namespace BingNew.Mapping.Interface;
public interface IDataSource
{
    public IEnumerable<object> MapMultipleObjects(List<CustomConfig> customConfigs);
    public string FetchData(Config config);
}
public interface IJsonDataSource : IDataSource
{

}
public interface IXmlDataSource : IDataSource
{

}
