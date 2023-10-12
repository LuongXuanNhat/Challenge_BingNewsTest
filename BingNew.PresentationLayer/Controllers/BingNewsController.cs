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
        public IActionResult BingNewsPanel(int quantity = 9)
        {
            var result = _bingNewsService.GetTrendingArticlesPanel(quantity);
            return result.Item1 ? Ok(result) : BadRequest(result);
        }

        [HttpGet("GetWeatherForecast")]
        public IActionResult WeatherForecast(DateTime? dateTime = null)
        {
            DateTime date = dateTime ?? DateTime.Now;
            var result = _bingNewsService.GetWeatherForecast(date);
            return result.Item1 ? Ok(result) : BadRequest(result);
        }
    }
}
