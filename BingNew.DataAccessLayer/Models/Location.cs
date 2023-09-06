namespace BingNew.DataAccessLayer.Models
{
    public class Location
    {
        public Guid Id { get; set; }
        public string PlaceName { get; set; }
        public int Longitude { get; set; }
        public int Latitude { get; set; }
        public Location()
        {
            Id = Guid.NewGuid();
            PlaceName = "";
            Longitude = 2;
            Latitude = 3;
        }


    }
}