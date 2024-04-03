
using SpiritsClassLibrary.DTOs.HabitatDTOs;
using SpiritsClassLibrary.Models;
using DemoSpiritsAPI.Servicies.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth;
using Newtonsoft.Json.Linq;

namespace DemoSpiritsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HabitatController : Controller
    {
        private IHabitatService _habitatService;
        private IGoogleAuthService _googleAuthService;


        public HabitatController(IHabitatService habitatService, IGoogleAuthService googleAuthService)
        {
            _habitatService = habitatService;
            _googleAuthService = googleAuthService;
        }
        [HttpGet(Name = "GetAllHabitats")]
        public IActionResult GetAll()
        {
            return Ok(_habitatService.GetAll());
        }

        [HttpGet("{id}", Name = "GetHabitatById")]
        public IActionResult Get(int id)
        {
            return Ok(_habitatService.Get(id));
        }

        [HttpDelete("{id}", Name = "DeleteHabitat")]
        public IActionResult Delete(int id)
        {
            var authResult = _googleAuthService.ValidateAdminPermission(Request);
            if (authResult.Exception != null)
            {
                return BadRequest(authResult.Exception.Message);
            }

            var result = _habitatService.Delete(id);
            if(result.Exception == null)
            {
                return Ok();
            }
            return BadRequest(result.Exception.Message);

        }

        [HttpPost(Name = "CreateHabitat")]
        public IActionResult Create([FromBody]CreateHabitatDTO createHabitatDTO)
        {
            var authResult = _googleAuthService.ValidateAdminPermission(Request);
            if (authResult.Exception != null)
            {
                return BadRequest(authResult.Exception.Message);
            }
            var result = _habitatService.Create(createHabitatDTO);
            if(result.Exception == null)
            {
                return Ok();
            }
            return BadRequest(result.Exception.Message);
        }

        [HttpPut(Name = "UpdateHabitat")]
        public IActionResult Update([FromBody] UpdateHabitatDTO updateHabitatDTO)
        {
            var authResult = _googleAuthService.ValidateAdminPermission(Request);
            if (authResult.Exception != null)
            {
                return BadRequest(authResult.Exception.Message);
            }
            var result = _habitatService.Update(updateHabitatDTO);
            if(result.Exception == null)
            {
                return Ok();
            }
            return BadRequest(result.Exception.Message);
        }

        [HttpPatch(Name = "SetupHabitatTestData")]
        public IActionResult SetupTestData()
        {
            var authResult = _googleAuthService.ValidateAdminPermission(Request);
            if (authResult.Exception != null)
            {
                return BadRequest(authResult.Exception.Message);
            }
            var result = _habitatService.SetupTestData();
            if (result.Exception == null)
            {
                return Ok();
            }
            return BadRequest(result.Exception.Message);
        }
    }
}
