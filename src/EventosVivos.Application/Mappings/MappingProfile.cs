using AutoMapper;
using EventosVivos.Application.DTOs;
using EventosVivos.Domain.Entities;

namespace EventosVivos.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Venue, VenueDto>()
                .ConstructUsing(src => new VenueDto(
                    src.Id,
                    src.Name,
                    src.Capacity,
                    src.City
                )).ForAllMembers(opt => opt.Ignore());

            CreateMap<Event, EventDto>()
                .ConstructUsing(src => new EventDto(
                    src.Id,
                    src.Title,
                    src.Description,
                    src.VenueId,
                    src.MaxCapacity,
                    src.AvailableTickets,
                    src.Schedule.StartTime,
                    src.Schedule.EndTime,
                    src.Price.Amount,
                    src.Type.ToString(),
                    src.CurrentStatus.ToString()
                )).ForAllMembers(opt => opt.Ignore());

            CreateMap<Reservation, ReservationDto>()
                .ConstructUsing(src => new ReservationDto(
                    src.Id,
                    src.EventId,
                    src.Quantity,
                    src.BuyerName,
                    src.BuyerEmail.Value,
                    src.Status.ToString(),
                    src.CreatedAt,
                    src.Code
                )).ForAllMembers(opt => opt.Ignore());
        }
    }
}
