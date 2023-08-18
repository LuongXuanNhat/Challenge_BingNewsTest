namespace BingNewsTest
{
    public class MicrosortNotification
    {
        private string _topic;
        private string _image;
        private TypeOfNotification _type;
        private string _noticeFrom;

        public MicrosortNotification(string topic, string image, TypeOfNotification type, string noticeFrom)
        {
            this._topic = topic;
            this._image = image;
            this._type = type;
            this._noticeFrom = noticeFrom;
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
            return this._topic;
        }

        internal TypeOfNotification GetTypes()
        {
            return _type;
        }
    }
}