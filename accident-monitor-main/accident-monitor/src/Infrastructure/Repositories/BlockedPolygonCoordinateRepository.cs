using AccidentMonitor.Application.Common.Interfaces;
using AccidentMonitor.Domain.Entities.MapStuff.Polygons;
using Microsoft.EntityFrameworkCore;

namespace AccidentMonitor.Infrastructure.Repositories;
public class BlockedPolygonCoordinateRepository(IApplicationDbContext context) : IBlockedPolygonCoordRepository
{
    private readonly IApplicationDbContext _context = context;
    public async Task<PolygonCoordinate> AddAsync(PolygonCoordinate entity)
    {
        await _context.PolygonCoordinates.AddAsync(entity);
        return entity;
    }

    public async Task<PolygonCoordinate?> DeleteAsync(Guid id)
    {
        var entity = await _context.PolygonCoordinates.FindAsync(id);
        if (entity != null)
        {
            _context.PolygonCoordinates.Remove(entity);
            return entity;
        }
        return null;
    }

    public async Task<List<PolygonCoordinate>> DeleteResolvedPolygonCoordinatesAsync(Guid accidentId)
    {
        var polygonCoordinates = await _context.PolygonCoordinates
            .Where(p => p.AccidentId == accidentId)
            .ToListAsync();

        _context.PolygonCoordinates.RemoveRange(polygonCoordinates);
        return polygonCoordinates;
    }

    public async Task<List<PolygonCoordinate>> GetAllAsync()
    {
        var polygonCoordinates = await _context.PolygonCoordinates
            .Include(p => p.Accident)
            .ToListAsync();
        return polygonCoordinates;
    }

    public async Task<List<PolygonCoordinate>> GetAllUnResolvedAsync()
    {
        var polygonCoordinates = await _context.PolygonCoordinates
            .Include(p => p.Accident)
            .Where(p => p.Accident.ResolvedStatus == Domain.Enums.AccidentResolvedStatus.Unresolved)
            .ToListAsync();
        return polygonCoordinates;
    }

    public async Task<PolygonCoordinate?> GetByIdAsync(Guid id)
    {
        var polygonCoordinate = await _context.PolygonCoordinates
            .Include(p => p.Accident)
            .FirstOrDefaultAsync(p => p.Guid == id);
        return polygonCoordinate;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<PolygonCoordinate?> UpdateAsync(PolygonCoordinate entity)
    {
        var polygonCoordinate = await _context.PolygonCoordinates.FindAsync(entity.Guid);
        if (polygonCoordinate != null)
        {
            polygonCoordinate.AccidentId = entity.AccidentId;
            polygonCoordinate.Coordinate = entity.Coordinate;
            _context.PolygonCoordinates.Update(polygonCoordinate);
            return polygonCoordinate;
        }
        return null;
    }
}
