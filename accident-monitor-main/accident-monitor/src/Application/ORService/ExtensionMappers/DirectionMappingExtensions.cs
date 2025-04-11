using AccidentMonitor.Application.ORService.Dto;
using AccidentMonitor.Application.ORService.Queries.GetDirectionAdvanced.Dtos;
using AccidentMonitor.Application.ORService.Queries.GetDirections.Dtos;
using AccidentMonitor.Domain.Entities.MapStuff;

namespace AccidentMonitor.Application.ORService.ExtensionMappers;
public static class DirectionMappingExtensions
{
    /// <summary>
    /// Maps a <see cref="GetDirectionAdvancedResponseDto"/> to a <see cref="DirectionCutResponseDto"/>.
    /// All segments from each route are aggregated into one list.
    /// </summary>
    /// <param name="source">The original routing response.</param>
    /// <returns>The mapped <see cref="DirectionCutResponseDto"/>.</returns>
    public static DirectionCutResponseDto ToDirectionCutResponseDto(this GetDirectionAdvancedResponseDto source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var directionCutResponse = new DirectionCutResponseDto()
        {
            Bbox = source.Bbox!.Select(Convert.ToDouble).ToList()
        };
        if (source.Routes != null && source.Routes.Count != 0)
        {
            var segments = source.Routes
                                 .Where(r => r?.Segments != null)
                                 .SelectMany(r => r.Segments)
                                 .ToList();

            directionCutResponse.Route = segments.Select(segment => new SegmentResponseDto
            {
                Steps = segment.Steps?.Select(step => new StepResponseDto(step.Distance, step.Duration)
                {
                    Instruction = step.Instruction ?? string.Empty
                }).ToList() ?? []

            }).ToList();
        }
        return directionCutResponse;
    }

    public static CoordinateDto ToCoordinateDto(this CoordinateEntity source) => new(source.Latitude, source.Longitude);

    public static CoordinateEntity ToCoordinateEntity(this CoordinateDto source) => new((float)source.Latitude, (float)source.Longitude);

    /// <summary>
    /// Maps a <see cref="StepDto"/> to a <see cref="StepResponseDto"/>.
    /// Cut the waypoints, type and name from the instruction
    /// </summary>
    /// <param name="source">The original step data transfer object.</param>
    public static StepResponseDto ToStepResponseDto(this StepDto source)
    {
        return new StepResponseDto(source.Distance, source.Duration)
        {
            Instruction = source.Instruction ?? string.Empty
        };
    }

    /// <summary>
    /// Maps a <see cref="GetDirectionResponseDto"/> to a <see cref="DirectionCutResponseDto"/>.
    /// Aggregates all segments from each feature into one list.
    /// </summary>
    /// <param name="source">The original direction response.</param>
    /// <returns>The mapped <see cref="DirectionCutResponseDto"/>.</returns>
    public static DirectionCutResponseDto ToDirectionCutResponseDto(this GetDirectionResponseDto source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var response = new DirectionCutResponseDto()
        {
            Bbox = source.Bbox!.Select(Convert.ToDouble).ToList()
        };

        if (source.Features != null && source.Features.Count != 0)
        {
            var segments = source.Features
                                 .Where(f => f?.Properties?.Segments != null)
                                 .SelectMany(f => f.Properties.Segments)
                                 .ToList();

            response.Route = segments.Select(segment => new SegmentResponseDto
            {
                Steps = segment.Steps?.Select(step => new StepResponseDto(step.Distance, step.Duration)
                {
                    Instruction = step.Instruction ?? string.Empty
                }).ToList() ?? new List<StepResponseDto>()
            }).ToList();
        }

        return response;
    }

}
