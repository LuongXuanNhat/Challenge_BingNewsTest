namespace BingNew.DataAccessLayer.Models
{
    public class Pagination
    {
        private int _total;
        private int _index;
        private int _size;
        private int _pageNumber;

        public Pagination()
        {

        }
        public Pagination(Pagination pagination)
        {
            _total = pagination._total;
            _index = pagination._index;
            _size = pagination._size;
            _pageNumber = (int)Math.Ceiling(_total * 1.0 / _size);
        }

        public Pagination(int total, int index, int size)
        {
            _total = total;
            _index = index;
            _size = size;
            _pageNumber = (int)Math.Ceiling(_total * 1.0 / _size);
        }

        internal int GetIndex()
        {
            return _index;
        }

        internal int GetSize()
        {
            return _size;
        }

        internal int GetTotal()
        {
            return _total;
        }

        public int GetPageNumber()
        {
            return _pageNumber;
        }
    }
}