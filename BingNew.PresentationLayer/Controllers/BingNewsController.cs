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
        private readonly DIContainer container = new DIContainer();
        private readonly IBingNewsService _bingNewsService;

        public BingNewsController() {
            _bingNewsService = container.Resolve<IBingNewsService>();
        }

        [HttpGet]
        public IEnumerable<Article> BingNewsPanel()
        {
            return _bingNewsService.GetTrendingArticlesPanel(9);
        }


    }
}
