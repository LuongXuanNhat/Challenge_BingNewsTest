public class MappingTable
{
    public string DestinationProperty { get; set; } = string.Empty;
    public string SourceProperty { get; set; } = string.Empty;

    public MappingTable(string SourceProperty, string DestinationProperty)
    {
        this.SourceProperty = SourceProperty;
        this.DestinationProperty = DestinationProperty;
    }


}