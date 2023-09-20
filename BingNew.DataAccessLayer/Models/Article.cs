using System.Collections;

namespace BingNew.DataAccessLayer.Models
{

    public class Article : BasePost
    {
        private int LikeNumber { get; set; }
        private int DisLikeNumber { get; set; }
        private int CommentNumber { get; set; }
        private int ViewNumber { get; set; } 
        private double Score { get; set; }
        private string ImgUrl { get; set; }
        private string Channel { get; set; }
        private string Category { get; set; }

        public Article() : base()
        {
            Score = 0;
            LikeNumber = 0;
            DisLikeNumber = 0;
            CommentNumber = 0;
            ImgUrl = "a";
            Channel = "b";
            Category = "c";
        }

        public float GetCommentNumber()
        {
            return CommentNumber;
        }

        public void SetCommentNumber(int commentNumber)
        {
            CommentNumber = commentNumber;
        }

        public float GetDisLikeNumber()
        {
            return DisLikeNumber;
        }

        public void SetDisLikeNumber(int disLikeNumber)
        {
            DisLikeNumber = disLikeNumber;
        }

        public float GetLikeNumber()
        {
            return LikeNumber;
        }

        public void SetLikeNumber(int likeNumber)
        {
            LikeNumber = likeNumber;
        }

        public double GetScore()
        {
            return Score;
        }
        public float GetViewNumber()
        {
            return ViewNumber;
        }
        public void SetViewNumber(int viewNumber)
        {
            ViewNumber = viewNumber;
        }
        public void SetScore(float value)
        {
            Score = value;
        }

        public string GetImgUrl()
        {
            return ImgUrl;
        }

        public string GetChannel()
        {
            return Channel;
        }
        public string GetCategory()
        {
            return Category;
        }

        public void SetCategory(string value)
        {
            Category = value;
        }

        public void SetChannel(string value)
        {
            Channel = value;
        }

        public void SetImgUrl(string value)
        {
            ImgUrl = value;
        }

    }
}