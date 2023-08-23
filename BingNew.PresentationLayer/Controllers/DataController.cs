using BingNew.BusinessLogicLayer.Services;
using BingNew.DataAccessLayer;
using BingNew.DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BingNew.DataAccessLayer.GetData;

namespace BingNew.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        [HttpGet("GetArticlesData")]
        public IActionResult GetArticlesData()
        {
            GetData getData = new GetData();

            string rssUrl = "https://vnexpress.net/rss/thoi-su.rss";
            string rssContent = getData.DownloadRssContent(rssUrl);
            List<Article> articles = getData.ParseRssContent(rssContent);
            return Ok(articles);
        }

        [HttpPost("CreateDatabase")]
        public IActionResult CreateDatabase()
        {
            try
            {
                DataService dataService = new DataService();
                dataService.RunDataOperations();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("Created database success");
        }
    }
}
