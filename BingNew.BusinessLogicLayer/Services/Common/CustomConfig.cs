using BingNew.BusinessLogicLayer.ModelConfig;
using System.ComponentModel.DataAnnotations;

namespace BingNew.BusinessLogicLayer.Services.Common
{
    public class CustomConfig
    {
        [Display(Name = "Tên bảng muốn mapping")]
        public string TableName { get; set; } = string.Empty;
        [Display(Name = "Danh sách thuộc tính")]
        public List<MappingTable> MappingTables { get; set; } = new List<MappingTable>();
        public Config Config { get; set; } = new();
    }
}