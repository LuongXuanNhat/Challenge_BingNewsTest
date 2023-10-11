namespace BingNew.DataAccessLayer.Entities
{
    public partial class WeatherVm
    {
        public WeatherVm() { }
        public WeatherVm(Weather weather, List<WeatherInfo> weatherInfor)
        {
            this.Id = weather.Id;
            this.Temperature = weather.Temperature;
            this.PubDate = weather.PubDate;
            this.Humidity = weather.Humidity;
            this.Place = weather.Place;
            this.Icon = weather.Icon;
            this.WeatherInfor = weatherInfor;
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Place { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public DateTime PubDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<WeatherInfo> WeatherInfor { get; set; } = new List<WeatherInfo>();
    }
}