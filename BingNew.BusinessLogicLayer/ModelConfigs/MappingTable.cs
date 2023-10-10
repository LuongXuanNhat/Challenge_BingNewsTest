using System.ComponentModel.DataAnnotations;

namespace BingNew.BusinessLogicLayer.ModelConfig;
public class MappingTable
{
    [Display(Name = "Đường dẫn dữ liệu nguồn thuộc tính")]
    public string SouPropertyPath { get; set; } = string.Empty;
    [Display(Name = "Kiểu dữ liệu nguồn")]
    public string SouDatatype { get; set; } = string.Empty;
    public string SouValue { get; set; } = string.Empty;
    [Display(Name = "Tên thuộc tính đích")]
    public string DesProperty { get; set; } = string.Empty;
    [Display(Name = "Kiểu dữ liệu thuộc tính đích")]
    public string DesDatatype { get; set; } = string.Empty;
    public string DesValue { get; set; } = string.Empty;
    [Display(Name = "Namespace yêu cầu (nếu có)")]
    public string Namespace { get; set; } = string.Empty;

}