using AccidentMonitor.Application.Common.Interfaces;
using AccidentMonitor.Application.Common.Mappings;
using AccidentMonitor.Application.Common.Models;
using AccidentMonitor.Domain.Entities.Accident;
using AccidentMonitor.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AccidentMonitor.Infrastructure.Repositories;
public class AccidentRepository(IApplicationDbContext context) : IAccidentRepository
{
    private readonly IApplicationDbContext _context = context;

    public async Task<AccidentEntity> AddAsync(AccidentEntity entity)
    {
        var result = await _context.Accidents.AddAsync(entity);
        return result.Entity;
    }

    public async Task<AccidentEntity?> DeleteAsync(Guid id)
    {
        var entity = await _context.Accidents.FindAsync(id);
        if (entity != null)
        {
            _context.Accidents.Remove(entity);
            return entity;
        }
        return null;
    }

    public async Task<List<AccidentEntity>> GetAllAsync()
    {
        var accidents = await _context.Accidents.ToListAsync();
        return accidents;
    }

    public async Task<AccidentEntity?> GetByIdAsync(Guid id)
    {
        var accident = await _context.Accidents
            //.Include(a => a.AccidentDetails)
            .Include(a => a.AccidentInvolved)
            .FirstOrDefaultAsync(a => a.Guid == id);
        return accident;
    }

    public async Task<Guid?> UpdateAccidentResolvedStatusAsync(Guid accidentId,
        AccidentResolvedStatus status)
    {
        var accident = await _context.Accidents.FindAsync(accidentId);
        if (accident != null)
        {
            accident.ResolvedStatus = status;
            _context.Accidents.Update(accident);
            return accidentId;
        }
        return null;
    }

    public async Task<AccidentEntity?> UpdateAsync(AccidentEntity entity)
    {
        var accident = await _context.Accidents.FindAsync(entity.Guid);
        if (accident != null)
        {
            accident.Timestamp = entity.Timestamp;
            accident.Latitude = entity.Latitude;
            accident.Longitude = entity.Longitude;
            accident.IsBlockingWay = entity.IsBlockingWay;
            accident.Severity = entity.Severity;
            accident.ResolvedStatus = entity.ResolvedStatus;
            _context.Accidents.Update(accident);
            return accident;
        }

        return null;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<AccidentEntity>> GetAllUnresolveAccident()
    {
        var accidents = await _context.Accidents
            .Where(a => a.ResolvedStatus == AccidentResolvedStatus.Unresolved)
            .ToListAsync();
        return accidents;
    }

    public async Task<PaginatedList<AccidentEntity>> GetUnresolvedAccidentWithPagination(int pageNumber, int pageSize)
    {
        return await _context.Accidents
            .Where(a => a.ResolvedStatus == AccidentResolvedStatus.Unresolved)
            .PaginatedListAsync(pageNumber, pageSize);
    }

    public Task<PaginatedList<AccidentEntity>> GetAccidentWithPagination(
        int pageNumber, int pageSize,
        bool isAscending = true
        )
    {

        var query = _context.Accidents.AsQueryable();

        if (isAscending)
        {
            query = query.OrderBy(a => a.Timestamp);
        }
        else
        {
            query = query.OrderByDescending(a => a.Timestamp);
        }
        return query.PaginatedListAsync(pageNumber, pageSize);
    }

    public Task<PaginatedList<AccidentEntity>> GetAccidentWithRangeOfDatePagination(DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize, bool isAscending = true)
    {
        var accidents = _context.Accidents
            .Where(a => a.Timestamp.Date >= startDate.ToDateTime(TimeOnly.MinValue) && a.Timestamp.Date <= endDate.ToDateTime(TimeOnly.MinValue));

        if (isAscending)
        {
            accidents = accidents.OrderBy(a => a.Timestamp);
        }
        else
        {
            accidents = accidents.OrderByDescending(a => a.Timestamp);
        }

        return accidents.PaginatedListAsync(pageNumber, pageSize);
    }
}
