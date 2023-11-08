////namespace BingNew.DataAccessLayer.Models
////{
////    public class BasePost
////    {
////        public BasePost()
////        {
////            Id = Guid.NewGuid();
////            ProviderId = string.Empty;
////            PubDate = DateTime.Now;
////            Url = string.Empty;
////            Title = string.Empty;
////            Description = string.Empty;
////        }
////        public BasePost(string providerId, DateTime pubDate, string url, string title, string description)
////        {
////            Id = Guid.NewGuid();
////            ProviderId = providerId;
////            PubDate = pubDate;
////            Url = url;
////            Title = title;
////            Description = description;
////        }

////        protected Guid Id;
////        protected string ProviderId;
////        protected DateTime PubDate;
////        protected string Url;
////        protected string Title;
////        protected string Description;

////        public Guid GetId()
////        {
////            return Id;
////        }
////        public DateTime GetPubDate()
////        {
////            return PubDate;
////        }
////        public string GetDescription()
////        {
////            return Description;
////        }

////        public void SetTitle(string newTitle)
////        {
////            Title = newTitle;
////        }
////        public string GetTitle()
////        {
////            return Title;   
////        }
////    }
////}