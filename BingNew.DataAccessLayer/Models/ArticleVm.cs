namespace BingNew.DataAccessLayer.Models
{

    public class ArticleVm : BasePost
    {
        private int LikeNumber;
        private int DisLikeNumber;
        private int CommentNumber ;
        private int ViewNumber ;
        private string ImgUrl;
        private string TopicId;


        public ArticleVm(int likeNumber, int disLikeNumber, int commentNumber, int viewNumber, string imgUrl,
         string category, string providerId, DateTime pubDate, string url, string title, string description) : base(providerId, pubDate, url,title,description)
        {
            LikeNumber = likeNumber;
            DisLikeNumber = disLikeNumber;
            CommentNumber = commentNumber;
            ViewNumber = viewNumber;
            ImgUrl = imgUrl;
            TopicId = category;
        }
        public ArticleVm() : base()
        {
            ImgUrl = string.Empty;
            TopicId = string.Empty;
        }
        public string GetImgUrl()
        {
            return ImgUrl;
        }
        public string GetCategory()
        {
            return TopicId;
        }

        public float GetCommentNumber()
        {
            return CommentNumber;
        }

        public float GetDisLikeNumber()
        {
            return DisLikeNumber;
        }

        public float GetLikeNumber()
        {
            return LikeNumber;
        }
        public float GetViewNumber()
        {
            return ViewNumber;
        }
        public void SetLikeNumber(int value)
        {
            LikeNumber = value;
        }

        public void SetDisLikeNumber(int value)
        {
            DisLikeNumber = value;
        }

        public void SetCommentNumber(int value)
        {
            CommentNumber = value;
        }

        public void SetViewNumber(int value)
        {
            ViewNumber = value;
        }
        public void SetImgUrl(string value)
        {
            ImgUrl = value;
        }

        public void SetCategory(string value)
        {
            TopicId = value;
        }
    }
}