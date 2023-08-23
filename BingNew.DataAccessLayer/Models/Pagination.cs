namespace BingNew.DataAccessLayer.Models
{
    public class Pagination
    {
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