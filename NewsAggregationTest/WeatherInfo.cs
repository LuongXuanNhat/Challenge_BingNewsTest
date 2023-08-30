public class WeatherInfo
{
    public Guid Id { get; set; }
    public double Temperature { get; set; } 
    public int WindSpeed { get; set; }
    public double AmountOfRain { get; set; }
    public int Hour { get; set; }
    public string WeatherIcon { get; set; } = string.Empty;
}