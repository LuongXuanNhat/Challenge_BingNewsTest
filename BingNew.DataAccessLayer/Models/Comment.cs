public class Comment
{
    public Guid Id { get; set; } = Guid.Empty;
    public string UserId { get; set; } = string.Empty;
    public Guid ArticleId { get; set; } = Guid.Empty;
    public string Content { get; set; } = string.Empty;
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