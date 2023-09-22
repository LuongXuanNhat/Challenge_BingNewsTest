using System.Collections;

namespace BingNew.DataAccessLayer.Models
{

    public class Article : BasePost
    {
        private int LikeNumber;
        private int DisLikeNumber;
        private int CommentNumber ;
        private int ViewNumber ;
        private double Score ;
        private string ImgUrl;
        private string Channel;
        private string Category;

        public Article() : base()
        {
            ImgUrl = string.Empty;
            Channel = string.Empty;
            Category = string.Empty;
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

        public double GetScore()
        {
            return Score;
        }
        public float GetViewNumber()
        {
            return ViewNumber;
        }

        public void SetChannel(string channel)
        {
            Channel = channel;
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

        public void SetScore(double value)
        {
            Score = value;
        }

        public void SetImgUrl(string value)
        {
            ImgUrl = value;
        }

        public void SetCategory(string value)
        {
            Category = value;
        }

    }
}