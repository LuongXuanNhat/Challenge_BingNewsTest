using System.Xml.Linq;

public class Config
{
    public Config()
    {
        Headers = new RequestHeaders();
        MappingTable = new List<MappingTable>();
    }

    public string Type { get; internal set; } 
    public string Url { get; internal set; }
    public RequestHeaders Headers { get; internal set; }
    public string Key { get; internal set; }
    public string KeyWork { get; internal set; }
    public string Language { get; internal set; }
    public string Category { get; internal set; }
    public string Channel { get; internal set; }
    public XNamespace NameSpace { get; internal set; } = "";
    public string DateTimeOffSetFormat { get; internal set; }
    public string Country { get; internal set; }
    public string ItemName { get; internal set; }
    internal List<MappingTable> MappingTable { get; set; }
}