using AutoMapper;
using EventosVivos.Application.DTOs;
using EventosVivos.Domain.Ports;
using MediatR;

namespace EventosVivos.Application.Features.Events.Queries
{
    public record GetEventByIdQuery(Guid Id) : IRequest<EventDto?>;

    public class GetEventByIdQueryHandler(IEventRepository eventRepository, IMapper mapper)
        : IRequestHandler<GetEventByIdQuery, EventDto?>
    {
        public async Task<EventDto?> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            var eventObj = await eventRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException("Evento no encontrado.");

            return mapper.Map<EventDto>(eventObj);
        }
    }
}
