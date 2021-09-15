using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ParkyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly DependencyService1 _dependencyService1;
        private readonly DependencyService2 _dependencyService2;

        public WeatherForecastController(ILogger<WeatherForecastController> logger ,
            DependencyService1 dependencyService1, 
            DependencyService2 dependencyService2)
        {
            _logger = logger;
            _logger.LogInformation("Hello from Logger");

            _dependencyService1 = dependencyService1;
            _dependencyService2 = dependencyService2;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();

            _dependencyService1.Write();
            _dependencyService2.Write();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}