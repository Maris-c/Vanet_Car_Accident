using System.Text.Json.Serialization;

namespace AccidentMonitor.Application.ORService.Dto;

/// <summary>
/// Represents a route in the routing response.
/// Contains summary, segments, and bounding box.
/// </summary>
/// <param name="Summary"> The summary of the route. </param>
/// <param name="Segments"> The list of segments in the route. </param>
/// <param name="Bbox"> The bounding box of the route as an array of numbers.
/// Format: [minLongitude, minLatitude, maxLongitude, maxLatitude] </param>
public record RouteDto([property: JsonPropertyName("summary")] SummaryDto? Summary, [property: JsonPropertyName("segments")] List<SegmentDto>? Segments, [property: JsonPropertyName("bbox")] double[]? Bbox);
