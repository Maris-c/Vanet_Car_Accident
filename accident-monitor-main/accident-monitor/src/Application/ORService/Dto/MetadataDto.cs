using System.Text.Json.Serialization;

namespace AccidentMonitor.Application.ORService.Dto;

/// <summary>
/// Represents the metadata of the routing response.
/// Contains attribution, service, timestamp, query, and engine.
/// </summary>
/// <param name="Attribution"> The attribution of the routing response. </param>
/// <param name="Service"> The service of the routing response. </param>
/// <param name="Timestamp"> The timestamp of the routing response. </param>
/// <param name="Query"> The query of the routing response. </param>
/// <param name="Engine"> The engine of the routing response. </param>
public record MetadataDto(
    [property: JsonPropertyName("attribution")] string? Attribution,
    [property: JsonPropertyName("service")] string? Service,
    [property: JsonPropertyName("timestamp")] long Timestamp,
    [property: JsonPropertyName("query")] QueryDto? Query,
    [property: JsonPropertyName("engine")] EngineDto? Engine);
