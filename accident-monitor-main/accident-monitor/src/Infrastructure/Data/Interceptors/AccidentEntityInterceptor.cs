using AccidentMonitor.Application.Helpers;
using AccidentMonitor.Domain.Entities.Accident;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AccidentMonitor.Infrastructure.Data.Interceptors;
public class AccidentEntityInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken)
    {
        var context = eventData.Context;
        if (context == null) return await base.SavingChangesAsync(eventData, result, cancellationToken);
        var newAccidents = context.ChangeTracker
            .Entries<AccidentEntity>()
            .Where(e => e.State == EntityState.Added)
            .Select(e => e.Entity)
            .ToList();

        foreach (var accident in newAccidents)
        {
            if (accident.IsBlockingWay)
            {
                accident.BlockedPolygonCoordinates = BlockPolygonGenerateHelper
                    .GenerateSquareCoordinates(accident);
            }
        }
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
