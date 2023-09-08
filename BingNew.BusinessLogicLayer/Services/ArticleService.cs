using BingNew.BusinessLogicLayer.Interfaces;
using BingNew.BusinessLogicLayer.Interfaces.IRepository;
using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.BusinessLogicLayer.ModelConfig;
using BingNew.BusinessLogicLayer.Services.Common;
using BingNew.DataAccessLayer.Models;
using BingNew.DataAccessLayer.Repositories;
using BingNew.DataAccessLayer.TestData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Dapper.SqlMapper;

namespace BingNew.BusinessLogicLayer.Services
{
    public class ArticleService : IArticleService
    {
        private readonly NewsService _newsService;
        private readonly DataSample _dataSample;
        private readonly IDataSource _apiDataSource;
        private readonly IDataSource _rssDataSource;
        private readonly IBaseRepository<Article> _articleRepository;

        public ArticleService()
        {
            _newsService = new NewsService();
            _dataSample = new DataSample();
            _apiDataSource = new ApiDataSource();
            _rssDataSource = new RssDataSource();
            _articleRepository = new ArticleRepository();
        }   
        public async Task<bool> Add(Article entity)
        {
            try
            {
               await _articleRepository.Add(entity);
            } catch (Exception e) {
                Debug.WriteLine("-------------------------------------------   BUG KÌA, FIX ĐI: " + e.Message.ToString());
                return false;
            }
            return true;
        }

        public async Task<bool> AddRange(IEnumerable<Article> articles)
        {
            foreach (var item in articles)
            {
                try
                {
                    await _articleRepository.Add(item);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("-------------------------------------------   BUG KÌA, FIX ĐI: " + e.Message.ToString());
                    return false;
                }
            }
            return true;
        }

        public List<Article> ConvertDataToArticles(Config config, List<MappingTable> mapping)
        {
            throw new NotImplementedException();
        }

        public Weather ConvertDataToWeather(string data, List<MappingTable> mapping)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                await _articleRepository.Delete(id);
            }
            catch (Exception e)
            {
                Debug.WriteLine("-------------------------------------------   BUG KÌA, FIX ĐI: " + e.Message.ToString());
                return false;
            }
            return true;
        }

        public async Task<IEnumerable<Article>> GetAll()
        {
            try
            {
                var result = await _articleRepository.GetAll();
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("-------------------------------------------   BUG KÌA, FIX ĐI: " + e.Message.ToString());
                return new List<Article>();
            }
        }

        public async Task<Article> GetById(string id)
        {
            try
            {
                var result = await _articleRepository.GetById(id);
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("-------------------------------------------   BUG KÌA, FIX ĐI: " + e.Message.ToString());
                return new Article();
            }
        }

        public string GetNews(string Url)
        {
            throw new NotImplementedException();
        }

        public string GetWeatherInfor(Config config)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(Article entity)
        {
            try
            {
                await _articleRepository.Update(entity);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("-------------------------------------------   BUG KÌA, FIX ĐI: " + e.Message.ToString());
                return false;
            }
        }

        public List<Article> UpdateArticlesFromTuoiTreNews(Config config)
        {
            try
            {
                config.Data = _rssDataSource.GetNews(config.Url);
                var result = _rssDataSource.ConvertDataToArticles(config, config.MappingTables);
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("-------------------------------------------   BUG KÌA, FIX ĐI: " + e.Message.ToString());
                return new List<Article>();
            }
        }
    }
}
