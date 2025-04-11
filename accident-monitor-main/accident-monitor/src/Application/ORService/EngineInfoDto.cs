using System.Text.Json.Serialization;

namespace AccidentMonitor.Application.ORService;

public class EngineInfoDto
{
    [JsonPropertyName("build_date")]
    public string BuildDate { get; set; } = string.Empty;

    [JsonPropertyName("graph_version")]
    public string GraphVersion { get; set; } = string.Empty;

    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;
}
