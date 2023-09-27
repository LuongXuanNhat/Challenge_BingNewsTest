namespace BingNew.DataAccessLayer.Models
{
    public class MicrosortNotification
    {
        private string Topic;
        private string Message;
        private TypeOfNotification Type;
        private string NoticeFrom;
        private string ImageLink;

        public MicrosortNotification()
        {
            Topic = string.Empty;
            Message = string.Empty;
            NoticeFrom = string.Empty;
            ImageLink = string.Empty;
        }
        public string GetTopic()
        {
            return Topic;
        }

        // Phương thức set cho Topic
        public void SetTopic(string value)
        {
            Topic = value;
        }

        // Phương thức get cho Message
        public string GetMessage()
        {
            return Message;
        }

        // Phương thức set cho Message
        public void SetMessage(string value)
        {
            Message = value;
        }

        // Phương thức get cho Type
        public TypeOfNotification GetTypes()
        {
            return Type;
        }

        // Phương thức set cho Type
        public void SetType(TypeOfNotification value)
        {
            Type = value;
        }

        // Phương thức get cho NoticeFrom
        public string GetNoticeFrom()
        {
            return NoticeFrom;
        }

        // Phương thức set cho NoticeFrom
        public void SetNoticeFrom(string value)
        {
            NoticeFrom = value;
        }

        // Phương thức get cho ImageLink
        public string GetImageLink()
        {
            return ImageLink;
        }

        // Phương thức set cho ImageLink
        public void SetImageLink(string value)
        {
            ImageLink = value;
        }
    }
}