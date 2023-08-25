public class MappingTable
{
    public string DestinationProperty { get; internal set; }
    public string SourceProperty { get; internal set; }

    public MappingTable(string SourceProperty, string DestinationProperty)
    {
        this.SourceProperty = SourceProperty;
        this.DestinationProperty = DestinationProperty;
    }


}