using System.Text.Json.Serialization;
using AccidentMonitor.Application.ORService.Dto;

namespace AccidentMonitor.Application.ORService.Queries.GetDirectionAdvanced.Dtos;

/// <summary>
/// Represents the root object of the routing response.
/// Contains bounding box, routes, and metadata.
/// </summary>
public class GetDirectionAdvancedResponseDto
{
    /// <summary>
    /// The bounding box of the route as an array of numbers.
    /// Format: [minLongitude, minLatitude, maxLongitude, maxLatitude]
    /// </summary>
    [JsonPropertyName("bbox")]
    public double[]? Bbox { get; set; }

    /// <summary>
    /// The list of routes in the response.
    /// </summary>
    [JsonPropertyName("routes")]
    public List<RouteDto>? Routes { get; set; }

    /// <summary>
    /// The metadata associated with the routing response.
    /// </summary>
    [JsonPropertyName("metadata")]
    public MetadataDto? Metadata { get; set; }
}
