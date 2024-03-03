using DemoSpiritsAPI.EntiryFramework.Contexts;
using DemoSpiritsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoSpiritsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private MySQLContext mySQLContext;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, MySQLContext context)
        {
            _logger = logger;
            mySQLContext = context;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {

            mySQLContext.Database.EnsureDeleted();
            mySQLContext.Database.EnsureCreated();

            Spirit spirit1 = new() { Id = 1 };
            Spirit spirit2 = new() { Id = 2 };

            Habitat habitat1 = new() { Id = 1 };
            Habitat habitat2 = new() { Id = 2 };

            spirit1.Habitats.Add(habitat1);
            spirit2.Habitats.Add(habitat2);

            habitat1.Spirits.Add(spirit1);
            habitat2.Spirits.Add(spirit2);

            mySQLContext.AddRange(spirit1, spirit2);
            mySQLContext.AddRange(habitat1, habitat2);

            mySQLContext.SaveChanges();

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
