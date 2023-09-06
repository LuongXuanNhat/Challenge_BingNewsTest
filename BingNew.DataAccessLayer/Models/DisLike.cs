namespace BingNew.DataAccessLayer.Models
{
    public class DisLike
    {
        public string UserId { get; set; } = string.Empty;
        public Guid ArticleId { get; set; } = Guid.Empty;
        public string Id { get; set; } = string.Empty;
    }
}