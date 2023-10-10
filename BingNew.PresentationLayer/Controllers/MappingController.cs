using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.BusinessLogicLayer.ModelConfig;
using BingNew.BusinessLogicLayer.Services.Common;
using BingNew.DI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BingNew.PresentationLayer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MappingController : ControllerBase
    {
        private readonly DIContainer _service;
        private readonly IApiDataSource _apiDataSource;
        private readonly IRssDataSource _rssDataSource;

        public MappingController(DIContainer container)
        {
            _service = container;
            _apiDataSource = _service.Resolve<IApiDataSource>(typeof(ApiDataSource).Name);
            _rssDataSource = _service.Resolve<IRssDataSource>(typeof(RssDataSource).Name);

        }

        [HttpPost("GetNews_Xml")]
        public IActionResult GetNewsFromApiWithTypeXml([FromBody] List<CustomConfig> customs )
        {
            try
            {
                foreach (var item in customs)
                {
                    var data = _rssDataSource.GetNews(item.Config.Url);
                    item.Config.Data = data;

                    _ = _rssDataSource.ConvertDataToArticles(item.Config, customs);
                }
                return Ok("Get News Data Success");
            }
            catch (Exception ex)
            {
                return BadRequest("Error | "+ ex.Message);
            }  
        }
        [HttpPost("GetNews_Json")]
        public IActionResult GetNewsFromApiWithTypeJson([FromBody] List<CustomConfig> customs )
        {
            try
            {
                foreach (var item in customs)
                {
                    var data = _apiDataSource.GetNews(item.Config.Url);
                    item.Config.Data = data;

                    _ = _apiDataSource.ConvertDataToArticles(item.Config, customs);
                }
                return Ok("Get News Data Success");
            }
            catch (Exception ex)
            {
                return BadRequest("Error | "+ ex.Message);
            }  
        }


    }
}