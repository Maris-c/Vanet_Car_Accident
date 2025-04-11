using System.Text.Json.Serialization;

namespace AccidentMonitor.Application.ORService.Dto;

/// <summary>
/// Represents the query of the routing response.
/// Contains coordinates, profile, profile name, format, units, language, and options.
/// </summary>
/// <param name="Coordinates"> The coordinates of the query as a list of arrays of numbers. </param>
/// <param name="Profile"> The profile of the query. </param>
/// <param name="ProfileName"> The profile name of the query. </param>
/// <param name="Format"> The format of the query. </param>
/// <param name="Units"> The units of the query. </param>
/// <param name="Language"> The language of the query. </param>
/// <param name="Options"> The options of the query. </param>
public record QueryDto(
    [property: JsonPropertyName("coordinates")] List<double[]>? Coordinates,
    [property: JsonPropertyName("profile")] string? Profile,
    [property: JsonPropertyName("profileName")] string? ProfileName,
    [property: JsonPropertyName("format")] string? Format,
    [property: JsonPropertyName("units")] string? Units,
    [property: JsonPropertyName("language")] string? Language,
    [property: JsonPropertyName("options")] OptionsDto? Options);
