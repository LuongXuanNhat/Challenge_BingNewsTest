using BingNew.BusinessLogicLayer.Interfaces;
using BingNew.DI;
using BingNew.Mapping;
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
        public bool GetNewsFromApiWithTypeXml([FromBody] List<CustomConfig> customs )
        {
            var result = _mappingService.CrawlNewsXml(customs);   
            return result;
        }

        [HttpPost("GetNews_Json")]
        public bool GetNewsFromApiWithTypeJson([FromBody] List<CustomConfig> customs)
        {
            var result = _mappingService.CrawlNewsJson(customs);
            return result;
        }

        [HttpPost("GetCrawlWeatherForecast")]
        public bool GetWeatherForecast([FromBody] List<CustomConfig> customs)
        {
            var result = _mappingService.CrawlWeatherForecast(customs);
            return result;
        }
    }
}