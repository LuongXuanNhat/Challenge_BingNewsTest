using BingNew.BusinessLogicLayer.Interfaces.IRepository;
using BingNew.DataAccessLayer.Models;
using Dapper;
using System.Data;

namespace BingNew.BusinessLogicLayer.Repositories
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly IDbConnection _dbConnection;

        public ProviderRepository()
        {
            _dbConnection = new DapperContext().CreateConnection();
        }

        public async Task Add(Provider provider)
        {
            _dbConnection.Open();
            string query = "INSERT INTO Provider (Id, name, icon, link ) " +
                 "VALUES (@Id, @ChannelName, @ChannelIcon, @Url)";
            await _dbConnection.ExecuteAsync(query, provider);
            _dbConnection.Close();
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Provider>> GetAll()
        {
            _dbConnection.Open();
            string query = "SELECT * FROM Provider";
            var result = await _dbConnection.QueryAsync<Provider>(query);
            _dbConnection.Close();
            return result;
        }

        public Task<Provider> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Provider entity)
        {
            throw new NotImplementedException();
        }

    
    }
}
