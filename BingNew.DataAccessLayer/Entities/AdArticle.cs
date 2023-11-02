namespace BingNew.DataAccessLayer.Entities
{
    public partial class AdArticle
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string MediaLink { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime PubDate { get; set; }
        public string Link { get; set; } = string.Empty;
        public string? ProviderId { get; set; }
    }
}