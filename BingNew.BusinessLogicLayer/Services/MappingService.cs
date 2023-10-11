using BingNew.BusinessLogicLayer.Interfaces;
using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.BusinessLogicLayer.Services.Common;
using BingNew.DataAccessLayer.Entities;
using BingNew.DI;
using BingNew.ORM.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingNew.BusinessLogicLayer.Services
{
    public class MappingService : IMappingService
    {

        private readonly IApiDataSource _apiDataSource;
        private readonly IRssDataSource _rssDataSource;
        private readonly DbBingNewsContext _dataContext;

        public MappingService(IApiDataSource apiDataSource, IRssDataSource rssDataSource, DbBingNewsContext dataContext)
        {
            _apiDataSource = apiDataSource;
            _rssDataSource = rssDataSource;
            _dataContext = dataContext;
        }

        public Tuple<bool, string> CrawlNewsJson(List<CustomConfig> customs)
        {
            try
            {
                var configs = customs.Select(item => item.Config).ToList();

                foreach (var config in configs)
                {
                    var data = _apiDataSource.GetNews(config.Url);
                    config.Data = data;

                    var result = _apiDataSource.ConvertDataToArticles<Article>(config, customs);
                    _dataContext.AddRanger(result);
                }
                return new Tuple<bool, string>(true, "Crawl news data successfully!");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "Error: " + ex.Message);
            }
        }

        public Tuple<bool, string> CrawlNewsXml(List<CustomConfig> customs)
        {
            try
            {
                var configs = customs.Select(item => item.Config).ToList();

                foreach (var config in configs)
                {
                    var data = _rssDataSource.GetNews(config.Url);
                    config.Data = data;

                    var result = _rssDataSource.ConvertDataToArticles<Article>(config, customs);
                    _dataContext.AddRanger(result);
                }
                return new Tuple<bool, string>( true, "Crawl news data successfully!");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "Error: " + ex.Message);
            }
        }
    }
}
