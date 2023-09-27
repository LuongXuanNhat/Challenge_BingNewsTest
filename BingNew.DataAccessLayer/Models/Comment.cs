namespace BingNew.DataAccessLayer.Models
{
    public class Comment
    {
        private Guid Id;
        private string UserId;
        private Guid ArticleId;
        private string Content;


        public Comment(string userId, Guid articleId, string content)
        {
            UserId = userId;
            ArticleId = articleId;
            Content = content;
            Id = Guid.NewGuid();
        }

        public Guid GetId()
        {
            return Id;
        }

        // Phương thức set cho Id
        public void SetId(Guid value)
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

        // Phương thức get cho Content
        public string GetContent()
        {
            return Content;
        }

        // Phương thức set cho Content
        public void SetContent(string value)
        {
            Content = value;
        }
    }
}