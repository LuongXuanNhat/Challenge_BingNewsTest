using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BingNew.BusinessLogicLayer.ModelConfig
{
    public class Config
    {
        public Config()
        {
            Headers = new RequestHeaders();
        }
        [Display(Name = "Đường dẫn lấy dữ liệu")]
        public string Url { get; set; } = string.Empty;
        [Display(Name = "Thông tin Header yêu cầu (nếu có)")]
        public RequestHeaders Headers { get; set; }
        [Display(Name = "Khóa Api")]
        public string? Key { get; set; } = string.Empty;
        [Display(Name = "Lọc theo từ khóa (nếu có)")]
        public string? KeyWork { get; set; } = string.Empty;
        [Display(Name = "Lọc theo ngôn ngữ (nếu có)")]
        public string? Language { get; set; } = string.Empty;
        [Display(Name = "Lọc theo danh mục (nếu có)")]
        public string? Category { get; set; } = string.Empty;
        [Display(Name = "Lọc theo kênh (nếu có)")]
        public string? Channel { get; set; } = string.Empty;
        [Display(Name = "Tên phần tử muốn lấy")]
        public string Item { get; set; } = string.Empty;
        [Display(Name = "Dành cho Api thời tiết (nếu có)")]
        public string? DayNumber { get; set; } = string.Empty;
        [Display(Name = "Dành cho Api thời tiết (nếu có)")]
        public string? Location { get; set; } = string.Empty;
        public string? Data { get; set; } = string.Empty;

    }
}