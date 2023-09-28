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
        public async Task<bool> Add(ArticleVm entity)
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

        public async Task<bool> AddRange(IEnumerable<ArticleVm> articles)
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

        public async Task<IEnumerable<ArticleVm>> GetAll()
        {
            try
            {
                var result = await _articleRepository.GetAll();
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("-------------------------------------------   BUG KÌA, FIX ĐI: " + e.Message.ToString());
                return new List<ArticleVm>();
            }
        }

        public async Task<ArticleVm> GetById(string id)
        {
            try
            {
                var result = await _articleRepository.GetById(id);
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("-------------------------------------------   BUG KÌA, FIX ĐI: " + e.Message.ToString());
                return new ArticleVm();
            }
        }

        public string GetNews(string Url)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(ArticleVm entity)
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

        public async Task<List<ArticleVm>> UpdateArticlesFromTuoiTreNews(Config config)
        {
            try
            {
                config.Data = _rssDataSource.GetNews(config.Url);
              ////  var result = _rssDataSource.ConvertDataToArticles(config, config.MappingTables);
                return await FilterArticles(new List<ArticleVm>());
            }
            catch (Exception e)
            {
                Debug.WriteLine("-------------------------------------------   BUG KÌA, FIX ĐI: " + e.Message.ToString());
                return new List<ArticleVm>();
            }
        }

        private async Task<List<ArticleVm>> FilterArticles(List<ArticleVm> result)
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
                return new List<ArticleVm>();
            }
            
        }
    }
}
