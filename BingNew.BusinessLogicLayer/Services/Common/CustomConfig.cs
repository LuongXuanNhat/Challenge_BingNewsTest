using BingNew.BusinessLogicLayer.ModelConfig;

namespace BingNew.BusinessLogicLayer.Services.Common
{
    public class CustomConfig
    {
        public string TableName { get; set; } = string.Empty;
        public List<MappingTable> MappingTables { get; set; } = new List<MappingTable>();
    }
}