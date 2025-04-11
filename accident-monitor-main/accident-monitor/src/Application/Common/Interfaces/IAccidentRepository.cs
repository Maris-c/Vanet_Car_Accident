using AccidentMonitor.Application.Common.Models;
using AccidentMonitor.Domain.Entities.Accident;
using AccidentMonitor.Domain.Enums;

namespace AccidentMonitor.Application.Common.Interfaces;
public interface IAccidentRepository : IRepository<AccidentEntity>
{
    Task<Guid?> UpdateAccidentResolvedStatusAsync(Guid accidentId, AccidentResolvedStatus status);
    Task<List<AccidentEntity>> GetAllUnresolveAccident();
    Task<PaginatedList<AccidentEntity>> GetUnresolvedAccidentWithPagination(int pageNumber, int pageSize);
    Task<PaginatedList<AccidentEntity>> GetAccidentWithPagination(int pageNumber, int pageSize, bool isAscending = true);
    Task<PaginatedList<AccidentEntity>> GetAccidentWithRangeOfDatePagination(DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize, bool isAscending = true);
}
