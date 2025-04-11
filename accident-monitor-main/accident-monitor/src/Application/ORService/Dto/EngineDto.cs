using System.Text.Json.Serialization;

namespace AccidentMonitor.Application.ORService.Dto;

/// <summary>
/// Represents the engine of the routing response.
/// Contains version, build date, and graph date.
/// </summary>
/// <param name="Version"> The version of the engine. </param>
/// <param name="BuildDate"> The build date of the engine. </param>
/// <param name="GraphDate"> The graph date of the engine. </param>
public record EngineDto([property: JsonPropertyName("version")] string? Version, [property: JsonPropertyName("build_date")] DateTime BuildDate, [property: JsonPropertyName("graph_date")] DateTime GraphDate);
