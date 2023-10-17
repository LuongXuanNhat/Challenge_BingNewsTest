using System.ComponentModel.DataAnnotations;

namespace BingNew.Mapping
{
    public class CustomConfig
    {
        public string TableName { get; set; } = string.Empty;
        public SingleOrList SingleMappingOrListMapping { get; set; }
        public string? SouPath { get; set; }
        public List<MappingTable> MappingTables { get; set; } = new List<MappingTable>();
        public Config Config { get; set; } = new();
    }
    public enum SingleOrList
    {
        Single,
        List
    }
}