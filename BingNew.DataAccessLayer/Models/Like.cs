namespace BingNew.DataAccessLayer.Models
{
    public class Like
    {
        public Like()
        {
            Id = string.Empty;
            UserId = string.Empty;
            ArticleId = Guid.Empty;
        }

        private string Id;
        private string UserId;
        private Guid ArticleId;

        public string GetId()
        {
            return Id;
        }

        // Phương thức set cho Id
        public void SetId(string value)
        {
            Id = value;
        }

        // Phương thức get cho UserId
        public string GetUserId()
        {
            return UserId;
        }

        // Phương thức set cho UserId
        public void SetUserId(string value)
        {
            UserId = value;
        }

        // Phương thức get cho ArticleId
        public Guid GetArticleId()
        {
            return ArticleId;
        }

        // Phương thức set cho ArticleId
        public void SetArticleId(Guid value)
        {
            ArticleId = value;
        }
    }
}