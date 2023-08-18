namespace BingNewsTest
{
    public class BasePost
    {
        private Guid _postId;
        private string _providerIcon;
        private string _providerName;
        private DateTime _postedTime;
        private string _title;

        public BasePost(string providerIcon, string providerName, DateTime postedTime, string title)
        {
            this._postId = Guid.NewGuid();
            this._providerIcon = providerIcon;
            this._providerName = providerName;
            this._postedTime = postedTime;
            this._title = title;
        }

        internal string GetPostedTime()
        {
            return _postedTime.ToString();
        }

        internal string GetPoviderIcon()
        {
            return _providerIcon;
        }

        internal string GetProviderName()
        {
            return _providerName;
        }

        internal string GetTitle()
        {
            return _title;
        }
    }
}