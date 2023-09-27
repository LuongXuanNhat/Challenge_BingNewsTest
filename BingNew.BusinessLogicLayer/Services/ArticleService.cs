using BingNew.BusinessLogicLayer.Interfaces.IRepository;
using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.BusinessLogicLayer.ModelConfig;
using BingNew.DataAccessLayer.Models;
using System.Diagnostics;

namespace BingNew.BusinessLogicLayer.Services
{
    public class ArticleService : IArticleService
    {
        ////private readonly float _viewMultiplier = 0.2f;
        ////private readonly float _likeMultiplier = 0.3f;
        ////private readonly float _commentMultiplier = 0.3f;
        ////private readonly float _disLikeMultiplier = 0.2f;
        ////private readonly int _trendingStoriesNumber = 9;

        private readonly IRssDataSource _rssDataSource;
        private readonly IArticleRepository _articleRepository;

        public ArticleService(IArticleRepository articleRepository,
                              IRssDataSource dataSource )
        {
            _articleRepository = articleRepository;
            _rssDataSource = dataSource;
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
          ////  await _channelService.FilterChannelsToAdd(articles);
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
                var latestNews = news.OrderBy(x => x.GetPubDate()).LastOrDefault();
                if (latestNews != null)
                {
                    return result.Where(x => x.GetPubDate() > latestNews.GetPubDate()).ToList();
                }
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
