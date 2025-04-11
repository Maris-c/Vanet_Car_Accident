using System.Text.Json.Serialization;

namespace AccidentMonitor.Application.ORService.Dto;

/// <summary>
/// Represents the summary of a route.
/// Contains distance and duration.
/// </summary>
/// <param name="Distance"> The distance of the route in meters. </param>
/// <param name="Duration"> The duration of the route in seconds. </param>
public record SummaryDto(
    [property: JsonPropertyName("distance")] double Distance,
    [property: JsonPropertyName("duration")] double Duration);
