using BingNew.BusinessLogicLayer.Interfaces;
using BingNew.BusinessLogicLayer.Interfaces.IRepository;
using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.BusinessLogicLayer.ModelConfig;
using BingNew.BusinessLogicLayer.Repositories;
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
        private readonly float _viewMultiplier = 0.2f;
        private readonly float _likeMultiplier = 0.3f;
        private readonly float _commentMultiplier = 0.3f;
        private readonly float _disLikeMultiplier = 0.2f;
        private readonly int _trendingStoriesNumber = 9;

        private readonly NewsService _newsService;
        private readonly DataSample _dataSample;
        private readonly IDataSource _apiDataSource;
        private readonly IDataSource _rssDataSource;
        private readonly IProviderService _channelService;
        private readonly IArticleRepository _articleRepository;

        public ArticleService()
        {
            _newsService = new NewsService();
            _dataSample = new DataSample();
            _apiDataSource = new ApiDataSource();
            _rssDataSource = new RssDataSource();
            _articleRepository = new ArticleRepository();
            _channelService = new ProviderService();
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
            await AddChannel(articles);
            try
            {
                foreach (var item in articles)
                {
                    await _articleRepository.Add(item);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("-------------------------------------------   BUG KÌA, FIX ĐI: " + e.Message.ToString());
                return false;
            }
            
            return true;
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

        public async Task<List<Article>> UpdateArticlesFromTuoiTreNews(Config config)
        {
            try
            {
                config.Data = _rssDataSource.GetNews(config.Url);
                var result = _rssDataSource.ConvertDataToArticles(config, config.MappingTables);
                return await FilterArticles(result);
            }
            catch (Exception e)
            {
                Debug.WriteLine("-------------------------------------------   BUG KÌA, FIX ĐI: " + e.Message.ToString());
                return new List<Article>();
            }
        }

        private async Task<List<Article>> FilterArticles(List<Article> result)
        {
            try
            {
                var news = await _articleRepository.GetAll();
                var latestNews = news.OrderBy(x => x.PubDate).LastOrDefault();
                if (latestNews != null)
                {
                    return result.Where(x => x.PubDate > latestNews.PubDate).ToList();
                }
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("-------------------------------------------   BUG KÌA, FIX ĐI: " + e.Message.ToString());
                return new List<Article>();
            }
            
        }

        public async Task<List<Article>> TrendingStories()
        {
            try
            {
                var articles = await _articleRepository.GetAll();
                DateTime specificDate = new DateTime(2023, 9, 12, 0, 0, 0, DateTimeKind.Utc);
                articles = articles.Where(x=>x.PubDate.Date == specificDate.Date).ToList();
                return GetTrendingStories(articles);
            }
            catch (Exception e)
            {
                Debug.WriteLine("-------------------------------------------   BUG KÌA, FIX ĐI: " + e.Message.ToString());
                return new List<Article>();
            }
        }

        private List<Article> GetTrendingStories(IEnumerable<Article> articles)
        {
            foreach (var item in articles)
            {
                item.Score = item.ViewNumber * _viewMultiplier + item.LikeNumber * _likeMultiplier + item.DisLikeNumber * _disLikeMultiplier + item.CommentNumber * _commentMultiplier;
            }
            return articles.OrderByDescending(x => x.Score).Take(_trendingStoriesNumber).ToList();
        }
    }
}
