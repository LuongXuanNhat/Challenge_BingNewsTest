namespace BingNew.DataAccessLayer.Models
{
    public class DisLike
    {
        private string UserId;
        private Guid ArticleId;
        private string Id;

        public DisLike() {
            UserId = string.Empty;
            ArticleId = Guid.Empty;
            Id = string.Empty;
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

        // Phương thức get cho Id
        public string GetId()
        {
            return Id;
        }

        // Phương thức set cho Id
        public void SetId(string value)
        {
            Id = value;
        }
    }
}