namespace BingNew.DataAccessLayer.Models
{
    public class Pagination
    {
        private Pagination pagination;

        public Pagination(Pagination pagination)
        {
            this.pagination = pagination;
        }

        public int Total { get; set; }
        public int Index { get; set; }
        public int Size { get; set; }
        public int PageNumber {
            get
            {
                return (int)Math.Ceiling((double)Total * 1.0 / Size);
            } set { } 
        } 

    }
}