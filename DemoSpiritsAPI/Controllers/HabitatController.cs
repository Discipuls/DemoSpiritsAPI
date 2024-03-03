
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
            _habitatService.Delete(id);
            return Ok();

        }

        [HttpPost(Name = "CreateHabitat")]
        public IActionResult Create([FromBody]CreateHabitatDTO createHabitatDTO)
        {
            _habitatService.Create(createHabitatDTO);
            return Ok();
        }

        [HttpPut(Name = "UpdateHabitat")]
        public IActionResult Update([FromBody] UpdateHabitatDTO updateHabitatDTO)
        {
            _habitatService.Update(updateHabitatDTO);
            return Ok();
        }
    }
}
