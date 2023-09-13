namespace BingNew.PresentationLayer.ViewModels
{
    public class ArticleVm
    {
        public string ProviderId { get; set; } = string.Empty;
        public DateTime PubDate { get; set; }
        public string Title { get; set; } = string.Empty;
        public int LikeNumber { get; set; }
        public int DisLikeNumber { get; set; }
    }
}
