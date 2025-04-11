using System.Text.Json.Serialization;

namespace AccidentMonitor.Application.ORService.Dto
{
    public class DirectionCutResponseDto
    {
        public List<double> Bbox { get; set; } = [];
        [JsonPropertyName("routes")]
        public List<SegmentResponseDto> Route { get; set; } = [];
    }

    public class SegmentResponseDto
    {
        public double Distance { get; set; }
        public double Duration { get; set; }
        public List<StepResponseDto>? Steps { get; set; } = [];
    }

    public record StepResponseDto(double Distance, double Duration)
    {
        public string Instruction { get; set; } = string.Empty;
    }
}
