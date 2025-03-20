using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Api.Controllers
{
    //[Authorize]//≈Á√“ Token
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IAuthorizationPolicyProvider _policyProvider;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("policies")]
        public async Task<IActionResult> GetPolicies()
        {
            var policies = new List<string>
            {
                "EditUserPolicy",
                "DeleteUserPolicy",
                "ViewReportsPolicy"
            };

            var availablePolicies = new List<string>();

            foreach (var policyName in policies)
            {
                var policy = await _policyProvider.GetPolicyAsync(policyName);
                if (policy != null)
                {
                    availablePolicies.Add(policyName);
                }
            }

            return Ok(JsonSerializer.Serialize(availablePolicies, new JsonSerializerOptions { WriteIndented = true }));
        }

    }
}
