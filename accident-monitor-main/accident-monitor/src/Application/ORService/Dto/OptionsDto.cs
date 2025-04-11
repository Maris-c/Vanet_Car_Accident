using System.Text.Json.Serialization;

namespace AccidentMonitor.Application.ORService.Dto;

/// <summary>
/// Represents the options of the query.
/// Contains avoid polygons.
/// </summary>
/// <param name="AvoidPolygons"> The avoid polygons of the options. </param>
public record OptionsDto([property: JsonPropertyName("avoid_polygons")] AvoidPolygonsDto? AvoidPolygons);
