namespace BingNew.DataAccessLayer.Models
{

    public class Article : BasePost
    {
        public int LikeNumber { get; set; }
        public int DisLikeNumber { get; set; }
        public int CommentNumber { get; set; }
        public int ViewNumber { get; set; }
        public double Score { get; set; }
        public string ImgUrl { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

        public Article()
        {

        }
    }
}