using BingNew.BusinessLogicLayer.Interfaces.IRepository;
using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.BusinessLogicLayer.Repositories;
using BingNew.DataAccessLayer.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace BingNew.BusinessLogicLayer.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository _providerRepository;
        public ProviderService()
        {
            _providerRepository = new ProviderRepository(); 
        }
        public async Task<bool> Add(Provider provider)
        {
            try
            {
                await _providerRepository.Add(provider);
            }
            catch (Exception e)
            {
                Debug.WriteLine("-------------------------------------------   BUG KÌA, FIX ĐI: " + e.Message.ToString());
                return false;
            }
            return true;
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task FilterChannelsToAdd(IEnumerable<Article> articles)
        {
            var providers = articles.Select(x => x.Channel).Distinct().ToList();
            var channels = await _providerRepository.GetAll();
            foreach (var item in providers)
            {
                if (channels.Any(x => x.ChannelName.Equals(item)))
                {
                    Provider newProvider = new Provider(item);
                    await _providerRepository.Add(newProvider);
                }
                
            }
        }

        public Task<IEnumerable<Provider>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Provider> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Provider entity)
        {
            throw new NotImplementedException();
        }
    }
}
