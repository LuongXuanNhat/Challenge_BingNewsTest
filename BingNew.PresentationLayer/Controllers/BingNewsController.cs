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
        // GET: api/<ArticleController>
        [HttpGet("Article")]
        public async Task<IEnumerable<Article>> GetArticle()
        {
            return await _articleService.GetAll();
        }

        // GET api/<ArticleController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
    }
}
