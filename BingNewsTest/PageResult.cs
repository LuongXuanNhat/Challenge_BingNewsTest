namespace BingNewsTest
{
    public class PageResult<T> : Pagination
    {
        public PageResult(Pagination pagination) : base(pagination) 
        { 
            Items = new List<T>();
        }

        public List<T> Items { get; set; }  
    }
}