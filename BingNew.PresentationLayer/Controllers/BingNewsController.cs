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

        public BingNewsController(DIContainer container) {
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
        public WeatherVm WeatherForecast(DateTime? dateTime = null)
        {
            DateTime date = dateTime ?? DateTime.Now;
            return _bingNewsService.GetWeatherForecast(date);
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
    }
}
