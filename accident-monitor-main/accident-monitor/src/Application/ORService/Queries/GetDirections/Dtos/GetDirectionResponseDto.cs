using System.Text.Json.Serialization;
using AccidentMonitor.Application.ORService.Dto;

namespace AccidentMonitor.Application.ORService.Queries.GetDirections.Dtos;

public class GetDirectionResponseDto
{
    [JsonPropertyName("type")]
    public string? Type { get; set; } = string.Empty;

    [JsonPropertyName("bbox")]
    public List<double>? Bbox { get; set; } = [];

    [JsonPropertyName("features")]
    public List<Feature>? Features { get; set; } = [];

    [JsonPropertyName("metadata")]
    public MetadataDto? Metadata { get; set; }
}
