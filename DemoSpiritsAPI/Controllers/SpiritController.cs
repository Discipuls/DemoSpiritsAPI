using AutoMapper;
using DemoSpiritsAPI.AutoMappers;
using SpiritsClassLibrary.DTOs.SpiritDTOs;
using SpiritsClassLibrary.Models;
using DemoSpiritsAPI.Servicies.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DemoSpiritsAPI.Servicies;

namespace DemoSpiritsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpiritController : Controller
    {

        private ISpiritService _spiritService;
        private IGoogleAuthService _googleAuthService;


        public SpiritController(ISpiritService spiritService, IGoogleAuthService googleAuthService)
        {
            _spiritService = spiritService;
            _googleAuthService = googleAuthService;
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

        [HttpPatch(Name = "SetupTestData")]
        public IActionResult SetupTestData()
        {
            var authResult = _googleAuthService.ValidateAdminPermission(Request);
            if (authResult.Exception != null)
            {
                return BadRequest(authResult.Exception.Message);
            }
            var result = _spiritService.SetupTestData();
            if (result.Exception == null)
            {
                return Ok();
            }
            return BadRequest(result.Exception.Message);
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
            var authResult = _googleAuthService.ValidateAdminPermission(Request);
            if (authResult.Exception != null && !authResult.Result)
            {
                return BadRequest(authResult.Exception.Message);
            }
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
            var authResult = _googleAuthService.ValidateAdminPermission(Request);
            if (authResult.Exception != null && !authResult.Result)
            {
                Exception ex = authResult.Exception;
                string message = "";
                while(ex != null)
                {
                    message += ex.Message + "\n";
                    ex = ex.InnerException;
                }
                return BadRequest(/*authResult.Exception.Message*/message);
            }
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
            var authResult = _googleAuthService.ValidateAdminPermission(Request);
            if (authResult.Exception != null && !authResult.Result)
            {
                return BadRequest(authResult.Exception.Message);
            }
            var result = _spiritService.Update(updateSpiritDTO);
            if(result.Exception == null)
            {
                return Ok();
            }

            return BadRequest(result.Exception.Message);
        }
    }
}
