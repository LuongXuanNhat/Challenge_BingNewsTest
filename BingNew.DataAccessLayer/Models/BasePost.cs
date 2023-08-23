using System.Text.Json.Serialization;

namespace BingNew.DataAccessLayer.Models
{
    public class BasePost
    {

        private string _id;
        private string _providerIcon;
        private string _providerName;
        private DateTimeOffset _pubDate;
        private string _link;
        private string _title;
        private string _description;

        public BasePost(string id,string title, DateTimeOffset date, string link, string description)
        {
            _id = id;
            _title = title;
            _pubDate = date;
            _link = link;
            _description = description;
            _providerIcon = "";
            _providerName = "";
        }

        internal string GetPostedTime()
        {
            return _pubDate.ToString();
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

        public string GetId()
        {
            return _id;
        }
    }
}