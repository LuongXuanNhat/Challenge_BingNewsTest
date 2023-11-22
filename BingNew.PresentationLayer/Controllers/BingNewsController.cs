 using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.DataAccessLayer.Entities;
using BingNew.DI;
using Microsoft.AspNetCore.Mvc;

namespace BingNew.PresentationLayer.Controllers
{
    [Route("BingNews")]
    [ApiController]
    public class BingNewsController : ControllerBase
    {
        private readonly IBingNewsService _bingNewsService;

        public BingNewsController(DIContainer container)
        {
            DIContainer _container = container;
            _bingNewsService = _container.Resolve<IBingNewsService>();
        }

        [HttpGet("GetNews")]
        public List<Article> BingNewsPanel(int quantity = 9)
        {
            var result = _bingNewsService.GetTrendingArticlesPanel(quantity);
            return result;
        }

        [HttpGet("GetWeatherForecast")]
        public async Task<WeatherVm> WeatherForecast(DateTime? dateTime = null)
        {
            DateTime date = dateTime ?? DateTime.Now;
            return await _bingNewsService.GetWeatherForecast(date);
        }

        [HttpGet("Search")]
        public List<Article> SearchNews(string keyWord)
        {
            return _bingNewsService.Search(keyWord);
        }

        [HttpPost("Advertisement")]
        public IActionResult AddAdvertisement(AdArticle ad)
        {
            _bingNewsService.AddAdvertisement(ad);
            return Ok();
        }
        [HttpGet("GetAllAdvertisement")]
        public List<AdArticle> GetAdArticle()
        {
            var result = _bingNewsService.GetAdArticles();
            return result;
        }

        [HttpPost("Like")]
        public IActionResult LikeArticle(UserInteraction userInteraction)
        {
            _bingNewsService.AddUserInteraction(userInteraction);
            return Ok();
        }

        [HttpPost("Dislike")]
        public IActionResult DislikeArticle(UserInteraction interaction)
        {
            _bingNewsService.AddUserInteraction(interaction);
            return Ok();
        }

        [HttpGet("FullTextSearch")]
        public IActionResult AdvancedSearch(string keyWord)
        {
            var result = _bingNewsService.FullTextSearch(keyWord);
            return Ok(result);
        }

        [HttpGet("Discover")]
        public IActionResult AdvancedSearch(Guid id)
        {
            var result = _bingNewsService.Recommendation(id);
            return Ok(result);
        }

        [HttpPost("RegisterUser")]
        public IActionResult RegisterUser(Users user)
        {
            var result = _bingNewsService.RegisterUser(user);
            return Ok(result);
        }
        
    }
}
