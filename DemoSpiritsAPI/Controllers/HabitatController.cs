
using DemoSpiritsAPI.DTOs.HabitatDTOs;
using DemoSpiritsAPI.Models;
using DemoSpiritsAPI.Servicies.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoSpiritsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HabitatController : Controller
    {
        private IHabitatService _habitatService;

        public HabitatController(IHabitatService habitatService)
        {
            _habitatService = habitatService;
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
            var result = _habitatService.SetupTestData();
            if (result.Exception == null)
            {
                return Ok();
            }
            return BadRequest(result.Exception.Message);
        }
    }
}
