namespace BingNew.DataAccessLayer.Models
{
    public class Weather
    {
        public Weather() {
            HourlyWeather = new List<WeatherInfo>();
        }
        private Guid Id;
        private string Place;
        private string Icon;
        private double Temperature;
        private int Humidity;
        private DateTime PubDate;
        private List<WeatherInfo> HourlyWeather;
        private string Description;

        public List<WeatherInfo> GetHourlyWeather()
        {
            return HourlyWeather;
        }

        public Guid GetId()
        {
            return Id;
        }

        public DateTime GetPubDate()
        {
            throw new NotImplementedException();
        }
    }
}