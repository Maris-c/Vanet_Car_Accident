using System.Text.Json.Serialization;

namespace AccidentMonitor.Application.ORService.Dto;

/// <summary>
/// Represents a step in a segment.
/// Contains distance, duration, type, instruction, name, and way points.
/// </summary>
public class StepDto
{
    [JsonPropertyName("distance")]
    public double Distance { get; set; }

    [JsonPropertyName("duration")]
    public double Duration { get; set; }

    [JsonPropertyName("type")]
    public int Type { get; set; }

    [JsonPropertyName("instruction")]
    public string? Instruction { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("way_points")]
    public int[]? WayPoints { get; set; }

    public StepDto()
    {

    }
    public StepDto(double distance, double duration, int type, string? instruction, string? name, int[]? wayPoints)
    {
        Distance = distance;
        Duration = duration;
        Type = type;
        Instruction = instruction;
        Name = name;
        WayPoints = wayPoints;
    }

    [JsonConstructor]
    public StepDto(double distance, double duration)
    {
        Distance = distance;
        Duration = duration;
    }
}
