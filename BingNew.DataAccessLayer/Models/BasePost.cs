using System.Text.Json.Serialization;

namespace BingNew.DataAccessLayer.Models
{
    public class BasePost
    {
        public string Id { get; set; } = string.Empty;
        public string ProviderId { get; set; } = string.Empty;
        public string PubDate { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty; 
        public string Title { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty;

       
    }
}