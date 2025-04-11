using AccidentMonitor.Application.Common.Interfaces;
using AccidentMonitor.Domain.Enums;

namespace AccidentMonitor.Application.Accident.Commands.UpdateResolveStatus;
public record UpdateAccidentResolvedStatusCommand : IRequest<Guid>
{
    public required Guid AccidentId { get; init; }
    public required bool IsResolved { get; init; }
}

public class UpdateAccidentResolvedStatusCommandHandler(
    IAccidentRepository accidentRepository,
    IBlockedPolygonCoordRepository polygonCoordinateRepository
    ) : IRequestHandler<UpdateAccidentResolvedStatusCommand, Guid>
{
    private readonly IAccidentRepository _accidentRepository = accidentRepository;
    private readonly IBlockedPolygonCoordRepository _polygonCoordinateRepository = polygonCoordinateRepository;

    public async Task<Guid> Handle(UpdateAccidentResolvedStatusCommand request, CancellationToken cancellationToken)
    {
        var accident = await _accidentRepository.GetByIdAsync(request.AccidentId);

        var status = request.IsResolved
            ? AccidentResolvedStatus.Resolved
            : AccidentResolvedStatus.Unresolved;

        if (accident == null) return Guid.Empty;
        if (accident.ResolvedStatus == status) return request.AccidentId;

        await _accidentRepository.UpdateAccidentResolvedStatusAsync(accident.Guid, status);

        //if (status == AccidentResolvedStatus.Resolved)
        //{
        //    await _polygonCoordinateRepository.DeleteResolvedPolygonCoordinatesAsync(request.AccidentId);
        //}

        await _accidentRepository.SaveChangesAsync(cancellationToken);
        return request.AccidentId;
    }
}
