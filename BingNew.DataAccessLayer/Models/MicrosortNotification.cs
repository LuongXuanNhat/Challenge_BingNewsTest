namespace BingNew.DataAccessLayer.Models
{
    public class MicrosortNotification
    {
        public string Topic { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public TypeOfNotification Type { get; set; }

        public string NoticeFrom { get; set; } = string.Empty;
            
        public string ImageLink { get; set; } = string.Empty;

       
    }
}