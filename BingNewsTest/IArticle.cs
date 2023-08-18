namespace BingNewsTest
{
    public interface IArticle
    {
        PageResult<Article> GetArticles(Pagination pagination);
    }
}