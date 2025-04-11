using System.Text.Json.Serialization;

namespace AccidentMonitor.Application.ORService.Dto;

public class Geometry
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("coordinates")]
    public List<List<double>> Coordinates { get; set; } = [];
}
