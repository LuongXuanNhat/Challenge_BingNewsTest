namespace BingNew.DataAccessLayer.Models
{
    public class WeatherInfo
    {

        private Guid Id;
        private double Temperature;
        private int Humidity;
        private int Hour;
        private string WeatherIcon = string.Empty;
        private Guid WeatherId;

        public void SetWeatherId(Guid id)
        {
            WeatherId = id;
            
        }
        public Guid GetId()
        {
            return Id;
        }

        public void SetId(Guid value)
        {
            Id = value;
        }

        public double GetTemperature()
        {
            return Temperature;
        }

        public void SetTemperature(double value)
        {
            Temperature = value;
        }

        public int GetHumidity()
        {
            return Humidity;
        }

        public void SetHumidity(int value)
        {
            Humidity = value;
        }

        public int GetHour()
        {
            return Hour;
        }

        public void SetHour(int value)
        {
            Hour = value;
        }

        public string GetWeatherIcon()
        {
            return WeatherIcon;
        }

        public void SetWeatherIcon(string value)
        {
            WeatherIcon = value;
        }

        public Guid GetWeatherId()
        {
            return WeatherId;
        }
    }
}