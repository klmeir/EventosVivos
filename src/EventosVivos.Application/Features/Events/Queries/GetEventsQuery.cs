using AutoMapper;
using EventosVivos.Application.DTOs;
using EventosVivos.Domain.Enums;
using EventosVivos.Domain.Ports;
using MediatR;

namespace EventosVivos.Application.Features.Events.Queries
{
    public record GetEventsQuery(
    EventType? Type,
    DateTime? StartDate,
    Guid? VenueId,
    EventStatus? Status,
    string? TitleSearch) : IRequest<List<EventDto>>;

    public class GetEventsQueryHandler(IEventRepository eventRepository, IMapper mapper)
        : IRequestHandler<GetEventsQuery, List<EventDto>>
    {
        public async Task<List<EventDto>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
        {            
            var events = await eventRepository.SearchAsync(
                request.Type, request.StartDate, request.VenueId, request.Status, request.TitleSearch);

            return mapper.Map<List<EventDto>>(events);
        }
    }
}
