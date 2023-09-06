namespace BingNew.DataAccessLayer.Models
{
    public class Weather
    {
        public Weather()
        {
            Id = Guid.NewGuid();
            HourlyWeather = new List<WeatherInfo>();
        }

        public Guid Id { get; set; }
        public string Place { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public string PubDate { get; set; } = string.Empty;
        public List<WeatherInfo> HourlyWeather { get; set; }
        public string Description { get; set; } = string.Empty;

    }
}