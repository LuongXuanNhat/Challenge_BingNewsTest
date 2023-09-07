using System.Text.Json.Serialization;

namespace BingNew.DataAccessLayer.Models
{
    public class BasePost
    {
        public BasePost()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string ProviderId { get; set; } = string.Empty;
        public DateTime PubDate { get; set; } 
        public string Url { get; set; } = string.Empty; 
        public string Title { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty;

       
    }
}