namespace BingNew.DataAccessLayer.Models
{
    public class Weather
    {
        public Weather() {
            HourlyWeather = new List<WeatherInfo>();
            Id = Guid.NewGuid();
            Place = string.Empty;
            Icon = string.Empty;
            Temperature = 0;
            Humidity = 0;
            PubDate = DateTime.Now;
            Description = string.Empty;
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

        public void SetHourlyWeather(List<WeatherInfo> value)
        {
            HourlyWeather = value;
        }
        public Guid GetId()
        {
            return Id;
        }

        public string GetPlace()
        {
            return Place;
        }

        // Phương thức set cho Place
        public void SetPlace(string value)
        {
            Place = value;
        }

        // Phương thức get cho Icon
        public string GetIcon()
        {
            return Icon;
        }

        // Phương thức set cho Icon
        public void SetIcon(string value)
        {
            Icon = value;
        }

        // Phương thức get cho Temperature
        public double GetTemperature()
        {
            return Temperature;
        }

        // Phương thức set cho Temperature
        public void SetTemperature(double value)
        {
            Temperature = value;
        }

        // Phương thức get cho Humidity
        public int GetHumidity()
        {
            return Humidity;
        }

        // Phương thức set cho Humidity
        public void SetHumidity(int value)
        {
            Humidity = value;
        }

        // Phương thức get cho PubDate
        public DateTime GetPubDate()
        {
            return PubDate;
        }

        // Phương thức set cho PubDate
        public void SetPubDate(DateTime value)
        {
            PubDate = value;
        }

        // Phương thức get cho Description
        public string GetDescription()
        {
            return Description;
        }

        // Phương thức set cho Description
        public void SetDescription(string value)
        {
            Description = value;
        }

    }
}