using AccidentMonitor.Application.BlockPolygon.Dtos;
using AccidentMonitor.Application.BlockPolygon.Queries;
using AccidentMonitor.Application.ORService.Dto;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AccidentMonitor.Web.Endpoints;

public class BlockedPolygon : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
        //.RequireAuthorization()
            .MapGet(GetBlockedPolygon, "/blocked-polygon")
            .MapGet(GetMultiBlockedPolygon, "/blocked-multi-polygon/");
    }
    public async Task<Results<Ok<List<BlockedPolygonDto>>, NotFound>> GetBlockedPolygon(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var blockedPolygons = await sender.Send(new GetBlockedPolygonsQuery(), cancellationToken);
        return blockedPolygons.Count == 0
            ? TypedResults.NotFound()
            : TypedResults.Ok(blockedPolygons);
    }

    public async Task<Results<Ok<AvoidPolygonsDto>, NotFound>> GetMultiBlockedPolygon(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var blockedPolygons = await sender.Send(new GetMultiBlockedPolygonFormat(), cancellationToken);
        return blockedPolygons == null
            ? TypedResults.NotFound()
            : TypedResults.Ok(blockedPolygons);
    }
}
