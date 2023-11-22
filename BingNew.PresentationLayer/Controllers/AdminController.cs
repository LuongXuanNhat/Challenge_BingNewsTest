using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.DataAccessLayer.Entities;
using BingNew.DI;
using Microsoft.AspNetCore.Mvc;

namespace BingNew.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IBingNewsService _bingNewsService;
        public AdminController(DIContainer container)
        {
            DIContainer _container = container;
            _bingNewsService = _container.Resolve<IBingNewsService>();
        }
        [HttpPost("AddRole")]
        public IActionResult AddRole(Role role)
        {
            var result = _bingNewsService.AddRole(role);
            return Ok(result);
        }
        [HttpPost("AddUserRole")]
        public IActionResult AddUserRole(UserRole userRole)
        {
            var result = _bingNewsService.AddUserRole(userRole);
            return Ok(result);
        }
        [HttpPut("UpdateUserRole")]
        public IActionResult UpdateUserRole(UserRole userRole)
        {
            var result = _bingNewsService.UpdateUserRole(userRole);
            return Ok(result);
        }
    }
}
