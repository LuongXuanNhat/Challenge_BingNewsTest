using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.DataAccessLayer.Entities;
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
        public IEnumerable<Article> BingNewsPanel()
        {
            return _bingNewsService.GetTrendingArticlesPanel(9);
        }


    }
}
