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
            List<GetSpiritBasicsDTO> spirits = [];
            try
            {
                spirits = _spiritService.GetAll();
            }catch (Exception ex) {
                return BadRequest(ex.Message);
            }
            return Ok(spirits);
        }

        [HttpGet("{id}", Name = "GetSpiritById")]
        public IActionResult Get(int id)
        {
            GetSpiritDTO getSpiritDTO = null;
            try
            {
                getSpiritDTO = _spiritService.Get(id);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(getSpiritDTO);
        }

        [HttpDelete("{id}", Name = "DeleteSpirit")]
        public IActionResult Delete(int id)
        {

            var result = _spiritService.Delete(id);

            if(result.Exception == null)
            {
                return Ok();
            }

            return BadRequest(result.Exception.Message);

        }

        [HttpPost(Name = "CreateSpirit")]
        public IActionResult Create([FromBody] CreateSpiritDTO createSpiritDTO)
        { 
          
            var result =  _spiritService.Create(createSpiritDTO);
            if(result.Exception == null)
            {
                return Ok();
            }

            return BadRequest(result.Exception.Message);
        }

        [HttpPut(Name = "UpdateSpirit")]
        public IActionResult Update([FromBody] UpdateSpiritDTO updateSpiritDTO)
        {

            var result = _spiritService.Update(updateSpiritDTO);
            if(result.Exception == null)
            {
                return Ok();
            }

            return BadRequest(result.Exception.Message);
        }
    }
}
