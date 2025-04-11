using System.Text.Json.Serialization;

namespace AccidentMonitor.Application.ORService.Dto;

public class Feature
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("bbox")]
    public List<double> Bbox { get; set; } = [];

    [JsonPropertyName("properties")]
    public Properties Properties { get; set; } = new Properties();

    [JsonPropertyName("geometry")]
    public Geometry Geometry { get; set; } = new Geometry();
}
