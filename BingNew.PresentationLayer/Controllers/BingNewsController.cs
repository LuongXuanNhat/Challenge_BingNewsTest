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
        private readonly DIContainer _bingNewsService;

        public BingNewsController(DIContainer container) {
            _bingNewsService = container;
        }

        [HttpGet]
        public IEnumerable<Article> BingNewsPanel()
        {
            var bingService = _bingNewsService.Resolve<IBingNewsService>();
            return bingService.GetTrendingArticlesPanel(9);
        }


    }
}
