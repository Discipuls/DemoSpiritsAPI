using AutoMapper;
using DemoSpiritsAPI.AutoMappers;
using DemoSpiritsAPI.DTOs.SpiritDTOs;
using DemoSpiritsAPI.Models;
using DemoSpiritsAPI.Servicies.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoSpiritsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpiritController : Controller
    {

        private ISpiritService _spiritService;

        public SpiritController(ISpiritService spiritService)
        {
            _spiritService = spiritService;
        }
        [HttpGet(Name = "GetAllSpirits")]
        public IActionResult GetAll()
        {
            return Ok(_spiritService.GetAll());
        }

        [HttpGet("{id}", Name = "GetSpiritById")]
        public IActionResult Get(int id)
        {
            return Ok(_spiritService.Get(id));
        }

        [HttpDelete("{id}", Name = "DeleteSpirit")]
        public IActionResult Delete(int id)
        {
            _spiritService.Delete(id);
            return Ok();

        }

        [HttpPost(Name = "CreateSpirit")]
        public IActionResult Create([FromBody] CreateSpiritDTO createSpiritDTO)
        {
            _spiritService.Create(createSpiritDTO);
            return Ok();
        }

        [HttpPut(Name = "UpdateSpirit")]
        public IActionResult Update([FromBody] UpdateSpiritDTO updateSpiritDTO)
        {
            _spiritService.Update(updateSpiritDTO);
            return Ok();
        }
    }
}
