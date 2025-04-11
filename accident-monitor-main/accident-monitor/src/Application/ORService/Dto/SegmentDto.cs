using System.Text.Json.Serialization;

namespace AccidentMonitor.Application.ORService.Dto;

/// <summary>
/// Represents a segment in a route.
/// Contains distance, duration, and steps.
/// </summary>
/// <param name="Distance"> The distance of the segment in meters. </param>
/// <param name="Duration"> The duration of the segment in seconds. </param>
/// <param name="Steps"> The list of steps in the segment. </param>
public record SegmentDto(
    [property: JsonPropertyName("distance")] double Distance,
    [property: JsonPropertyName("duration")] double Duration,
    [property: JsonPropertyName("steps")] List<StepDto>? Steps);
