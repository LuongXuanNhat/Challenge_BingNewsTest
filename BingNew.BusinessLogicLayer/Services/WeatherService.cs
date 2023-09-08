using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingNew.BusinessLogicLayer.Services
{
    public class WeatherService : IWeatherService
    {
        public async Task<bool> Add(Weather entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Weather>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Weather> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(Weather entity)
        {
            throw new NotImplementedException();
        }
    }
}
