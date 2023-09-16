namespace BingNew.DataAccessLayer.Models
{
    public class Location
    {
        private Guid Id;
        private string PlaceName;
        private int Longitude;
        private int Latitude;
        public Location()
        {
            Id = Guid.NewGuid();
            PlaceName = "";
            Longitude = 2;
            Latitude = 3;
        }


    }
}