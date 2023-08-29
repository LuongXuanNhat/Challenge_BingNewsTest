namespace BingNew.DataAccessLayer.Models
{
    public class MicrosortNotification
    {
        public string? Topic { get; set; }
        public string? Message { get; set; }
        public TypeOfNotification Type { get; set; }

        public string? NoticeFrom { get; set; }

        public string? ImageLink { get; set;}

       
    }
}