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
        public Guid GetId()
        {
            return Id;
        }

        // Phương thức set cho Id
        public void SetId(Guid value)
        {
            Id = value;
        }

        // Phương thức get cho PlaceName
        public string GetPlaceName()
        {
            return PlaceName;
        }

        // Phương thức set cho PlaceName
        public void SetPlaceName(string value)
        {
            PlaceName = value;
        }

        // Phương thức get cho Longitude
        public int GetLongitude()
        {
            return Longitude;
        }

        // Phương thức set cho Longitude
        public void SetLongitude(int value)
        {
            Longitude = value;
        }

        // Phương thức get cho Latitude
        public int GetLatitude()
        {
            return Latitude;
        }

        // Phương thức set cho Latitude
        public void SetLatitude(int value)
        {
            Latitude = value;
        }

    }
}