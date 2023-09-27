using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace BingNew.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BingNewsController : ControllerBase
    {
        private readonly IArticleService _articleService;
        public BingNewsController(IArticleService articleService) {
            _articleService = articleService;
        }
        // GET: api/Article
        [HttpGet("Article")]
        public async Task<IEnumerable<Article>> GetArticle()
        {
            return await _articleService.GetAll();
        }

        //// GET api/TrendingStories
        ////[HttpGet("TrendingStories")]
        ////public async Task<IEnumerable<Article>> GetTrendingStories()
        ////{
        ////    return await _articleService.TrendingStories();
        ////}
    }
}
