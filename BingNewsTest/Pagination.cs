using System.Drawing;

namespace BingNewsTest
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
            this._total = pagination._total; 
            this._index = pagination._index;
            this._size = pagination._size;
            this._pageNumber = (int)Math.Ceiling(_total * 1.0 / _size);
        }

        public Pagination(int total, int index, int size)
        {
            this._total = total;
            this._index = index;
            this._size = size;
            this._pageNumber = (int)Math.Ceiling(_total * 1.0 / _size);
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