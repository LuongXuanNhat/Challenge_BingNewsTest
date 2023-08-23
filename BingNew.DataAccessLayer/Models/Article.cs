using System.Text.Json.Serialization;

namespace BingNew.DataAccessLayer.Models
{

    public class Article : BasePost
    {
        private int _likeNumber;
        private int _disLikeNumber;
        private int _commentNumber;
        private string _image;



        public Article(string id, string title, DateTimeOffset pubDate, string link, string description, string urlImage) : base(id, title,pubDate, link, description) 
        {
            _commentNumber = 0;
            _likeNumber = 0;
            _disLikeNumber = 0;
            _likeNumber = 0;
            _image = urlImage;
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