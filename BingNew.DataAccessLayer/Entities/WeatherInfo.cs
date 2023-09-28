namespace BingNew.DataAccessLayer.Entities
{
    public partial class WeatherInfo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public double? Temperature { get; set; }
        public int? Humidity { get; set; }
        public int? Hour { get; set; }
        public string? WeatherIcon { get; set; }
        public string? WeatherId { get; set; }
    }
}