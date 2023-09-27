using BingNew.BusinessLogicLayer.Interfaces.IRepository;
using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.DataAccessLayer.Models;
using System.Diagnostics;

namespace BingNew.BusinessLogicLayer.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherRepository _weatherRepository;

        public WeatherService(IWeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository;
        }


        public async Task<bool> Add(Weather entity)
        {
            try
            {
                if (await CheckDate(entity.GetPubDate()))
                {
                    await _weatherRepository.Add(entity);
                    await AddRangeWeatherHour(entity.GetHourlyWeather());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("-------------------------------------------   BUG KÌA, FIX ĐI: " + e.Message.ToString());
                return false;
            }
            return true;
        }

        private async Task<bool> CheckDate(DateTime pubDate)
        {
            var weathers = await _weatherRepository.GetAll();
            var lastestWeather = weathers.OrderByDescending(x => x.GetPubDate()).FirstOrDefault();
            if (lastestWeather != null)
            {
                if (lastestWeather.GetPubDate().DayOfYear < pubDate.DayOfYear)
                    return true;
                return false;
            }
            return true;
        }

        public async Task<bool> AddRangeWeatherHour(List<WeatherInfo> hourlyWeather)
        {
            try
            {
                foreach (var item in hourlyWeather)
                {
                    await _weatherRepository.AddWeatherHour(item);
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("BUG KÌA, FIX ĐI: " + e.Message.ToString());
                return false;
            }
            
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                await _weatherRepository.Delete(id);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("BUG KÌA, FIX ĐI: " + e.Message.ToString());
                return false;
            }
        }

        public async Task<IEnumerable<Weather>> GetAll()
        {
            try
            {
                var result = await _weatherRepository.GetAll();
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("BUG KÌA, FIX ĐI: " + e.Message.ToString());
                return new List<Weather>();
            }
        }

        public async Task<Weather> GetById(string id)
        {
            try
            {
                var result = await _weatherRepository.GetById(id);
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("BUG KÌA, FIX ĐI: " + e.Message.ToString());
                return new Weather(); 
            }
        }

        public async Task<bool> Update(Weather entity)
        {
            try
            {
                await _weatherRepository.Update(entity);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("BUG KÌA, FIX ĐI: " + e.Message.ToString());
                return false;
            }
        }

    }
}
