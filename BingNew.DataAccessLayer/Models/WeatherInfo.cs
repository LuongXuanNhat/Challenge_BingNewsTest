namespace BingNew.DataAccessLayer.Models
{
    public class WeatherInfo
    {

        private Guid Id;
        private double Temperature;
        private int Humidity;
        private int Hour;
        private string WeatherIcon;
        private Guid WeatherId;

        public Guid GetWeatherId()
        {
            return WeatherId;
        }

        public void SetWeatherId(Guid id)
        {
            WeatherId = id;
        }
    }
}