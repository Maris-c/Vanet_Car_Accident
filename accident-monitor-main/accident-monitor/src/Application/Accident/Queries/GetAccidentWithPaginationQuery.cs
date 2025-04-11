using AccidentMonitor.Application.Accident.Dtos;
using AccidentMonitor.Application.Common.Interfaces;
using AccidentMonitor.Application.Common.Models;

namespace AccidentMonitor.Application.Accident.Queries;
public record GetAccidentWithPaginationQuery : IRequest<PaginatedList<AccidentDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public DateOnly StartDate { get; init; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly EndDate { get; init; } = DateOnly.FromDateTime(DateTime.Now);
    public bool IsAscending { get; init; } = true;
}

public class GetAccidentWithPaginationQueryHandler(
    IAccidentRepository accidentRepository, IMapper mapper)
    : IRequestHandler<GetAccidentWithPaginationQuery, PaginatedList<AccidentDto>>
{
    public async Task<PaginatedList<AccidentDto>> Handle(GetAccidentWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var accidents = await accidentRepository.GetAccidentWithRangeOfDatePagination(
            request.StartDate,
            request.EndDate,
            request.PageNumber,
            request.PageSize,
            request.IsAscending);

        var accidentDtos = mapper.ProjectTo<AccidentDto>(accidents.Items.AsQueryable());

        return await PaginatedList<AccidentDto>.CreateAsync(
            accidentDtos,
            request.PageNumber,
            request.PageSize);
    }
}
