namespace BingNew.DataAccessLayer.Models
{
    public class WeatherInfo
    {
        public Guid Id { get; set; }
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public double AmountOfRain { get; set; }
        public int Hour { get; set; }
        public string WeatherIcon { get; set; } = string.Empty;
    }
}