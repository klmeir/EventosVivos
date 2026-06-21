using EventosVivos.Application.Features.Venues.Queries;
using MediatR;

namespace EventosVivos.Api.Endpoints
{
    public static class VenueEndpoints
    {
        public static RouteGroupBuilder MapVenueEndpoints(this RouteGroupBuilder group)
        {            
            group.MapGet("/", async (ISender mediator) =>
                Results.Ok(await mediator.Send(new GetVenuesQuery())));

            return group;
        }
    }
}
