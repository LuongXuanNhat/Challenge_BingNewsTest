using System.Collections;

namespace BingNew.DataAccessLayer.Models
{

    public class Article : BasePost
    {
        private int LikeNumber;
        private int DisLikeNumber;
        private int CommentNumber;
        private int ViewNumber;
        private double Score;
        private string ImgUrl;
        private string Channel;
        private string Category;

        public Article() : base()
        {
            
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

        public void SetScore(float value)
        {
            Score = value;
        }

    }
}