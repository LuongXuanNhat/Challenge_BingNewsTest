using System.ComponentModel.DataAnnotations;

namespace BingNew.Mapping
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