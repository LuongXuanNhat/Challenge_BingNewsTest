using BingNew.BusinessLogicLayer.Interfaces;
using BingNew.BusinessLogicLayer.Services.Common;
using BingNew.DI;
using Microsoft.AspNetCore.Mvc;

namespace BingNew.PresentationLayer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MappingController : ControllerBase
    {
        private readonly IMappingService _mappingService;

        public MappingController()
        {
            DIContainer _service = new();
            _mappingService = _service.Resolve<IMappingService>();
        }

        [HttpPost("GetNews_Xml")]
        public IActionResult GetNewsFromApiWithTypeXml([FromBody] List<CustomConfig> customs )
        {
            var result = _mappingService.CrawlNewsXml(customs);   
            return result.Item1 ? Ok(result.Item2) : BadRequest(result.Item2);
        }

        [HttpPost("GetNews_Json")]
        public IActionResult GetNewsFromApiWithTypeJson([FromBody] List<CustomConfig> customs)
        {
            var result = _mappingService.CrawlNewsJson(customs);
            return result.Item1 ? Ok(result.Item2) : BadRequest(result.Item2);
        }

        [HttpPost("GetCrawlWeatherForecast")]
        public IActionResult GetWeatherForecast([FromBody] List<CustomConfig> customs)
        {
            var result = _mappingService.CrawlWeatherForecast(customs);
            return result.Item1 ? Ok(result.Item2) : BadRequest(result.Item2);
        }
    }
}