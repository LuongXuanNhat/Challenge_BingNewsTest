using System.ComponentModel.DataAnnotations;

namespace BingNew.Mapping
{
    public class CustomConfig
    {
        public string TableName { get; set; } = string.Empty;
        public MappingType SingleMappingOrListMapping { get; set; }
        public string? SouPath { get; set; }
        public List<MappingTable> MappingTables { get; set; } = new();
        public Config Config { get; set; } = new();
    }
    public enum MappingType
    {
        Single,
        List
    }
}