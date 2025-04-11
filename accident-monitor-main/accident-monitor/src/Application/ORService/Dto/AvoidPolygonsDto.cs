using System.Text.Json.Serialization;

namespace AccidentMonitor.Application.ORService.Dto;

/// <summary>
/// Represents the avoid polygons of the options.
/// Contains coordinates and type.
/// </summary>
/// <param name="Coordinates"> The coordinates of the avoid polygons as a list of arrays of arrays of numbers. </param>
/// <param name="Type"> The type of the avoid polygons. </param>
public record AvoidPolygonsDto(
    [property: JsonPropertyName("coordinates")] List<List<List<CoordinateDto>>>? Coordinates,
    [property: JsonPropertyName("type")] string? Type);
