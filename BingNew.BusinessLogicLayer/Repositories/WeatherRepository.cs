using BingNew.BusinessLogicLayer.Interfaces.IRepository;
using BingNew.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingNew.BusinessLogicLayer.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
        public async Task Add(Weather entity)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string id)
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

        public async Task Update(Weather entity)
        {
            throw new NotImplementedException();
        }
    }
}
