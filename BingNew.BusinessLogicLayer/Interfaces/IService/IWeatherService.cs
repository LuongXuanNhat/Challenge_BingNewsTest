using BingNew.DataAccessLayer.Models;

namespace BingNew.BusinessLogicLayer.Interfaces.IService
{
    public interface IWeatherService : IBaseService<Weather>
    {
        Task<bool> AddRangeWeatherHour(List<WeatherInfo> hourlyWeather);
    }
}
