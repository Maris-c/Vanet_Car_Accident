using System.Text.Json.Serialization;
using AccidentMonitor.Application.Common.Interfaces;
using AccidentMonitor.Application.Helpers;
using AccidentMonitor.Domain.Entities.Accident;
using AccidentMonitor.Domain.Enums;

namespace AccidentMonitor.Application.Accident.Commands.CreateAccidentReport;
public record CreateAccidentOnReportCommand : IRequest<Guid>
{
    [JsonPropertyName("time")]
    public required DateTime Time { get; init; }
    [JsonPropertyName("longitude")]
    public double Latitude { get; init; }
    [JsonPropertyName("latitude")]
    public double Longitude { get; init; }
    [JsonPropertyName("severity")]
    public AccidentSeverity Severity { get; init; }
    [JsonPropertyName("is_blocking_way")]
    public bool IsBlockingWay { get; init; }
}
public class CreateAccidentReportCommandHandler(IAccidentRepository repository) : IRequestHandler<CreateAccidentOnReportCommand, Guid>
{
    private readonly IAccidentRepository _repository = repository;
    public async Task<Guid> Handle(CreateAccidentOnReportCommand request, CancellationToken cancellationToken)
    {
        var accident = new AccidentEntity()
        {
            Timestamp = request.Time,
            Latitude = (float)request.Latitude,
            Longitude = (float)request.Longitude,
            IsBlockingWay = request.IsBlockingWay,
            Severity = request.Severity,
            ResolvedStatus = AccidentResolvedStatus.Unresolved
        };

        if (request.IsBlockingWay)
        {
            accident.BlockedPolygonCoordinates = BlockPolygonGenerateHelper.GenerateSquareCoordinates(accident);
        }
        await _repository.AddAsync(accident);

        //accident.AddDomainEvent(new AccidentEntityCreatedEvent(accident));
        await _repository.SaveChangesAsync(cancellationToken);
        return accident.Guid;
    }
}


