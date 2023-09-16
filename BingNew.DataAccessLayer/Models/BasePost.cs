using System.Text.Json.Serialization;

namespace BingNew.DataAccessLayer.Models
{
    public class BasePost
    {
        public BasePost()
        {
            Id = Guid.NewGuid();
        }

        protected Guid Id;
        protected string ProviderId;
        protected DateTime PubDate;
        protected string Url;
        protected string Title;
        protected string Description;

        public Guid GetId()
        {
            return Id;
        }
        public DateTime GetPubDate()
        {
            return PubDate;
        }
        public string GetDescription()
        {
            return Description;
        }

        public void SetTitle(string newTitle)
        {
            Title = newTitle;
        }
        public string GetTitle()
        {
            return Title;   
        }
    }
}