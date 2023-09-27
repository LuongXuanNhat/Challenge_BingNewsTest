using Microsoft.AspNetCore.Mvc;
namespace BingNew.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MappingController : ControllerBase
    {

        ////private readonly IArticleService _articleService;

        ////public MappingController(IArticleService articleService) {
        ////    _articleService = articleService;
        ////}

        ////[HttpPost("UpdateArticlesFromTuoiTreNews")]
        ////public async Task<List<Article>> UpdateArticlesFromTuoiTreNews(Config config)
        ////{
        ////    var result = await _articleService.UpdateArticlesFromTuoiTreNews(config);
        ////    await _articleService.AddRange(result);
        ////    return result;
        ////}

        ////[HttpPost("UpdateArticlesFromNewsDataIo")]
        ////public async Task<List<Article>> UpdateArticlesFromNewsDataIo(Config config)
        ////{
        ////    var result = await _articleService.UpdateArticlesFromTuoiTreNews(config);
        ////    await _articleService.AddRange(result);
        ////    return result;
        ////}

    }
}
