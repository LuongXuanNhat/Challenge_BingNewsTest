using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.BusinessLogicLayer.Interfaces;
using BingNew.DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using BingNew.BusinessLogicLayer.Services.Common;
using BingNew.BusinessLogicLayer.Services;

namespace BingNew.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BingNewsController : ControllerBase
    {
        private readonly IArticleService _articleService;
        public BingNewsController() {
            _articleService = new ArticleService();
        }
        // GET: api/Article
        [HttpGet("Article")]
        public async Task<IEnumerable<Article>> GetArticle()
        {
            return await _articleService.GetAll();
        }

        // GET api/TrendingStories
        [HttpGet("TrendingStories")]
        public async Task<IEnumerable<Article>> GetTrendingStories()
        {
            return await _articleService.TrendingStories();
        }
    }
}
