namespace BingNew.DataAccessLayer.Models
{
    public class Weather
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Place { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public DateTime PubDate { get; set; }
        public List<WeatherInfo> HourlyWeather { get; set; } = new List<WeatherInfo>();
        public string Description { get; set; } = string.Empty;

    }
}