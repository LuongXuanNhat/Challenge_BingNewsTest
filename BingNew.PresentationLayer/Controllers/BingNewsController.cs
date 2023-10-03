using BingNew.BusinessLogicLayer.Interfaces.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BingNew.PresentationLayer.Controllers
{
    [Route("BingNews")]
    [ApiController]
    public class BingNewsController : ControllerBase
    {
        private readonly IBingNewsService _bingNewsService;

        public BingNewsController(IBingNewsService bingNewsService) {
            _bingNewsService = bingNewsService;
        }

        [HttpGet]
        public IActionResult BingNewsPanel()
        {
            var result = _bingNewsService.GetTrendingArticlesPanel(9);
            return Ok(result);
        }
    }
}
