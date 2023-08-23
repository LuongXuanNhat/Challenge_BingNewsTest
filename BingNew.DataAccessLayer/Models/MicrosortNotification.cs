namespace BingNew.DataAccessLayer.Models
{
    public class MicrosortNotification
    {
        private string _topic;
        private string _image;
        private TypeOfNotification _type;
        private string _noticeFrom;

        public MicrosortNotification(string topic, string image, TypeOfNotification type, string noticeFrom)
        {
            _topic = topic;
            _image = image;
            _type = type;
            _noticeFrom = noticeFrom;
        }

        internal string GetImage()
        {
            return _image;
        }

        internal string GetNoticeFrom()
        {
            return _noticeFrom;
        }

        internal string GetTopic()
        {
            return _topic;
        }

        internal TypeOfNotification GetTypes()
        {
            return _type;
        }
    }
}