using BingNew.BusinessLogicLayer.Interfaces.IRepository;
using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.DataAccessLayer.Models;
using System.Diagnostics;

namespace BingNew.BusinessLogicLayer.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository _providerRepository;
        public ProviderService(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository; 
        }
        public async Task<bool> Add(ProviderVm provider)
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

        ////public async Task FilterChannelsToAdd(IEnumerable<ArticleVm> articles)
        ////{
        ////    var providers = articles.Select(x => x.GetChannel()).Distinct().ToList();
        ////    var channels = await _providerRepository.GetAll();
        ////    foreach (var item in providers)
        ////    {
        ////        if (!channels.Any(x => x.GetChannelName().Equals(item)))
        ////        {
        ////            ProviderVm newProvider = new ProviderVm(item.ToString());
        ////            await _providerRepository.Add(newProvider);
        ////        }
                
        ////    }
        ////}

        public Task<IEnumerable<ProviderVm>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ProviderVm> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ProviderVm entity)
        {
            throw new NotImplementedException();
        }
    }
}
