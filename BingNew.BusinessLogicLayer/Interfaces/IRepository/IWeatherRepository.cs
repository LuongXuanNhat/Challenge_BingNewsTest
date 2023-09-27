using BingNew.DataAccessLayer.Models;

namespace BingNew.BusinessLogicLayer.Interfaces.IRepository
{
    public interface IWeatherRepository : IBaseRepository<Weather>
    {
        Task AddWeatherHour(WeatherInfo weatherInfo);
    }
}
