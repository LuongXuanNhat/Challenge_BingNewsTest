using System.Text.Json.Serialization;

namespace BingNew.DataAccessLayer.Models
{
    public class BasePost
    {
        public BasePost()
        {
            Id = Guid.NewGuid();
            ProviderId = "1";
            PubDate = DateTime.Now;
            Url = "a";
            Title = "b";
            Description = "c";
        }
        protected Guid Id { get; set; }
        protected string ProviderId { get; set; }
        protected DateTime PubDate { get; set; }
        protected string Url { get; set; }
        protected string Title { get; set; }
        protected string Description { get; set; }

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