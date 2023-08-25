using System.Text.Json.Serialization;

namespace BingNew.DataAccessLayer.Models
{
    public class BasePost
    {
        public string Id { get; set; }
        public Guid ProviderId { get; set; }
        public DateTimeOffset PubDate { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

       
    }
}