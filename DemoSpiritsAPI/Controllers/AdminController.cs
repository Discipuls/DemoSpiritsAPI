using DemoSpiritsAPI.Servicies;
using DemoSpiritsAPI.Servicies.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoSpiritsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private IGoogleAuthService _googleAuthService;
        public AdminController(IGoogleAuthService googleAuthService)
        {
            _googleAuthService = googleAuthService;
        }

        [HttpGet(Name = "IsAdmin")]
        public IActionResult IsAdmin()
        {
            var authResult = _googleAuthService.ValidateAdminPermission(Request);
            return Ok(authResult.Result);

        }
    }
}
