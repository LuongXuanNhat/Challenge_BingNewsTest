public class Weather
{
    public Weather()
    {
        Id = Guid.NewGuid();
        HourlyWeather = new List<WeatherInfo>();
    }

    public Guid Id { get; set; }
    public string Place { get; set; }
    public string PubDate { get; set; }
    public List<WeatherInfo> HourlyWeather { get; set; }
    public string Description { get; set; } = string.Empty;
    
}