namespace BingNewsTest
{
    public class AdArtile : BasePost
    {
        private string _image;

        public AdArtile(string providerIcon, string providerName, DateTime postedTime, string title, string image) : base(providerIcon, providerName, postedTime, title)
        {
            this._image = image;
        }

        internal string GetImage()
        {
            return _image;
        }
    }
}