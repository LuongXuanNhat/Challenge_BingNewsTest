﻿public class Like
{
    public Like()
    {
    }

    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public Guid ArticleId { get; set; } = Guid.Empty;
}