namespace BingNewsTest
{
    public class Article : BasePost
    {
        private int _likeNumber;
        private int _disLikeNumber;
        private int _commentNumber;
        private string _image;


        public Article(string providerIcon, string providerName, DateTime postedTime, string title, string image, int likeNumber, int disLikeNumber, int commentNumber) : base(providerIcon, providerName, postedTime, title)
        {
            _image = image;
            _likeNumber = likeNumber;
            _disLikeNumber = disLikeNumber;
            _commentNumber = commentNumber;
            
        }

        internal int GetCommentNumber()
        {
            return _commentNumber;
        }

        internal int GetDislikeNumber()
        {
            return _disLikeNumber;
        }

        internal string GetImage()
        {
            return _image;
        }

        internal int GetLikeNumber()
        {
            return _likeNumber;
        }


    }
}