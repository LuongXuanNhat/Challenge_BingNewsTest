namespace BingNew.DataAccessLayer.Models
{

    public class Article : BasePost
    {
        public int LikeNumber { get; set; }
        public int DisLikeNumber { get; set; }
        public int CommentNumber { get; set; }
        public string ImgUrl { get; set; }
        public string Channel { get; set; }
        public string Category { get; set; }

        //public Article(string? title, string? link, string? description, DateTimeOffset pubDate, string? imgUrl)
        //{
        //    Title = title;
        //    Url = link;
        //    Description = description;
        //    ImgUrl = imgUrl;
        //    PubDate = pubDate;
        //}

        public Article()
        {
        }
    }
}