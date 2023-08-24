using System.Text.Json.Serialization;

namespace BingNew.DataAccessLayer.Models
{
    public class BasePost
    {
        public string Id { get; set; }
        public string ProviderId { get; set; }
        public DateTimeOffset? PubDate { get; set; }
        public string Link;
        public string Title { get; set; }
        public string Description { get; set; }


       
    }
}