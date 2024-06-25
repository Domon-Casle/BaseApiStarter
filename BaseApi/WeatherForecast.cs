using CoreUtilities.Logger.Attributes;
using System.Text.Json.Serialization;

namespace BaseApi
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }

        [JsonIgnore]
        public string JsonIgnored { get; set; } = "should be ignred";

        [DoNotLog]
        public string LoggerIgnored { get; set; } = "logger should ignore";
    }
}
