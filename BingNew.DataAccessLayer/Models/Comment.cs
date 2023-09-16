namespace BingNew.DataAccessLayer.Models
{
    public class Comment
    {
        private Guid Id;
        private string UserId;
        private Guid ArticleId;
        private string Content;
        public Comment()
        {
        }

        public Comment(string userId, Guid articleId, string content)
        {
            UserId = userId;
            ArticleId = articleId;
            Content = content;
            Id = Guid.NewGuid();
        }
    }
}