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

        public string GetPlace()
        {
            return Place;
        }

        public string GetIcon()
        {
            return Icon;
        }
        public double GetTemperature()
        {
            return Temperature;
        }
        public int GetHumidity()
        {
            return Humidity;
        }
        public DateTime GetPubDate()
        {
            return PubDate;
        }
        public string GetDesciption()
        {
            return Description;
        }
        public List<WeatherInfo> GetWeatherInfo()
        {
            return HourlyWeather;
        }


    }
}