using Microsoft.ArmSwashbuckleStarterKit.Attributes;
using Microsoft.ArmSwashbuckleStarterKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ArmSwashbuckleStarterKit.Controllers
{
    [ApiController]
    [Route("subscriptions/{subscriptionId:guid}/resourceGroups/{resourceGroupName}/providers/Microsoft.Weather/forecasts")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Fetches a weather forecast
        /// </summary>
        /// <param name="subscriptionId">The ID of the subscription that the forecast belongs to</param>
        /// <param name="resourceGroupName">>The name of the resource group the forecast belongs to</param>
        /// <param name="forecastId">The public ID of the forecast</param>
        /// <returns>The requested forecast</returns>
        [HttpGet("{forecastId}")]
        [ProducesResponseType(typeof(WeatherForecast), StatusCodes.Status200OK)]
        [SwaggerLinkToExample]
        public WeatherForecast Get(Guid subscriptionId, string resourceGroupName, Guid forecastId)
        {
            // the random values used below are just for the sake of example. In reality, this GET would be deterministic
            var rng = new Random();
            return new WeatherForecast
            {
                Date = DateTime.Now,
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            };
        }

        /// <summary>
        /// Fetches a forecast and the next 4 days' forecasts
        /// </summary>
        /// <param name="subscriptionId">The ID of the subscription that the forecast belongs to</param>
        /// <param name="resourceGroupName">>The name of the resource group the forecast belongs to</param>
        /// <param name="forecastId">The public ID of the forecast</param>
        /// <returns>The requested forecast</returns>
        [HttpGet("{forecastId}/fiveDay")]
        [ProducesResponseType(typeof(WeatherForecast), StatusCodes.Status200OK)]
        [SwaggerLinkToExample]
        public ResourceListResultModel<WeatherForecast> List(Guid subscriptionId, string resourceGroupName, Guid forecastId)
        {
            // the random values used below are just for the sake of example. In reality, this GET would be deterministic
            var rng = new Random();
            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            return new ResourceListResultModel<WeatherForecast>
            {
                Value = forecasts
            };
        }
    }
}
