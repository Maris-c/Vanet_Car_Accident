using System.Text.Json.Serialization;

namespace AccidentMonitor.Application.ORService.Dto;

public class Properties
{
    [JsonPropertyName("segments")]
    public List<SegmentDto> Segments { get; set; } = [];

    [JsonPropertyName("way_points")]
    public List<int> WayPoints { get; set; } = [];

    [JsonPropertyName("summary")]
    public SummaryDto? Summary { get; set; }
}
