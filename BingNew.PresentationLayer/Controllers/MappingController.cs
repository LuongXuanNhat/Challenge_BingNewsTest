using BingNew.BusinessLogicLayer.Interfaces;
using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.BusinessLogicLayer.ModelConfig;
using BingNew.BusinessLogicLayer.Services;
using BingNew.BusinessLogicLayer.Services.Common;
using BingNew.DataAccessLayer.Models;
using BingNew.DataAccessLayer.TestData;
using Microsoft.AspNetCore.Mvc;
namespace BingNew.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MappingController : ControllerBase
    {

        private readonly IArticleService _articleService;

        public MappingController(IArticleService articleService) {
            _articleService = articleService;
        }

        // GET: api/<MappingController>
        [HttpPost("GetArticlesOfTuoiTreNews")]
        public string GetArticlesOfTuoiTreNews(string? url = "https://tuoitre.vn/rss/tin-moi-nhat.rss")
        {
           return "s";
        }
        [HttpPost("AddArticles")]
        public string AddArticle()
        {
            var a = new Article();
            _articleService.Add(a);
           return "s";
        }

        [HttpPost("UpdateArticlesFromTuoiTreNews")]
        public async Task<List<Article>> UpdateArticlesFromTuoiTreNews(Config config)
        {
            var result = await _articleService.UpdateArticlesFromTuoiTreNews(config);
            await _articleService.AddRange(result);
            return result;
        }

        [HttpPost("UpdateArticlesFromNewsDataIo")]
        public async Task<List<Article>> UpdateArticlesFromNewsDataIo(Config config)
        {
            var result = await _articleService.UpdateArticlesFromTuoiTreNews(config);
            await _articleService.AddRange(result);
            return result;
        }

    }
}
