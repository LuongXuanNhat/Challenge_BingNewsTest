using BingNew.BusinessLogicLayer.DapperContext;
using BingNew.BusinessLogicLayer.Interfaces.IRepository;
using BingNew.DataAccessLayer.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace BingNew.BusinessLogicLayer.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly IDbConnection _dbConnection;

        public WeatherRepository()
        {
            _dbConnection = new DbContext().CreateConnection();
        }

        public async Task Add(Weather entity)
        {
            _dbConnection.Open();
            string query = "INSERT INTO Weather (Id, Place, Icon, Description, PubDate, Temperature, Humidity) " +
                 "VALUES (@Id, @Place, @Icon, @Description, @PubDate, @Temperature, @Humidity)";
            await _dbConnection.ExecuteAsync(query, entity);
            _dbConnection.Close();
        }

        public async Task AddWeatherHour(WeatherInfo weatherInfo)
        {
            _dbConnection.Open();
            string query = "INSERT INTO WeatherInfo (Id, Temperature, Humidity, Hour, WeatherIcon) " +
                 "VALUES (@Id, @Temperature, @Humidity, @Hour , @WeatherIcon)";
            await _dbConnection.ExecuteAsync(query, weatherInfo);
            _dbConnection.Close();
        }

        public async Task Delete(string id)
        {
            _dbConnection.Open();
            string query = "DELETE FROM Weather WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(query, new { Id = id });
            _dbConnection.Close();
        }

        public async Task<IEnumerable<Weather>> GetAll()
        {
            _dbConnection.Open();
            string query = "SELECT * FROM Weather";
            var result = await _dbConnection.QueryAsync<Weather>(query);
            _dbConnection.Close();
            return result;
        }

        public async Task<Weather> GetById(string id)
        {
            _dbConnection.Open();
            string query = "SELECT * FROM Weather WHERE Id = @Id";
            var result = await _dbConnection.QueryFirstOrDefaultAsync<Weather>(query, new { Id = id });
            _dbConnection.Close();
            return result;
        }

        public async Task<bool> Update(Weather weather)
        {
            _dbConnection.Open();
            string selectQuery = $@"SELECT * FROM Weather WHERE id = '{weather.GetId()}'";
            var entity = await _dbConnection.QueryAsync<Weather>(selectQuery, weather.GetId());
            if (entity is null)
                return false;

            string query = "UPDATE Weather " +
                           "SET Place = @Place, Icon = @Icon, Description = @Description, " +
                           "PubDate = @PubDate, Temperature = @Temperature, Humidity = @Humidity " +
                           "WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(query, entity);
            _dbConnection.Close();
            return true;
        }

    }
}
