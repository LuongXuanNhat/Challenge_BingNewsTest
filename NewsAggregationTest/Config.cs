using System.Xml.Linq;

public class Config
{
    public Config()
    {
        Headers = new RequestHeaders();
        MappingTable = new List<MappingTable>();
    }

    public string Type { get; set; }  = string.Empty;
    public string Url { get; set; } = string.Empty;
    public RequestHeaders Headers { get; set; }
    public string Key { get; set; } = string.Empty;
    public string KeyWork { get; set; } = string.Empty; 
    public string Language { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public XNamespace Namespace { get; set; } = "";
    public string DateTimeOffSetFormat { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Item { get; set; } = string.Empty;
    public string NewsItems { get; set; } = string.Empty;
    internal List<MappingTable> MappingTable { get; set; }
}