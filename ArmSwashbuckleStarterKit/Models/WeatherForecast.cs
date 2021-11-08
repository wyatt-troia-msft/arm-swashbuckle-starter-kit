using Microsoft.ArmSwashbuckleStarterKit.Attributes;
using System;

namespace ArmSwashbuckleStarterKit
{
    [SwaggerIsTrackedARMResourceAttribute]
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        [SwaggerExclude]
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }

        public Guid PublicId { get; set; }
    }
}
