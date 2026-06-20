using EventosVivos.Domain.Exceptions;
using EventosVivos.Domain.Ports;
using EventosVivos.Domain.ValueObjects;

namespace EventosVivos.Domain.Services
{
    public class VenueAvailabilityChecker
    {
        private readonly IEventRepository _eventRepository;

        public VenueAvailabilityChecker(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task EnsureIsAvailableAsync(Guid venueId, TimeFrame schedule)
        {
            bool hasOverlap = await _eventRepository.HasOverlappingEventsAsync(
                venueId,
                schedule.StartTime,
                schedule.EndTime
            );

            if (hasOverlap)
            {
                throw new DomainRuleValidationException("RN-02: The selected venue already has an active event that overlaps with this schedule.");
            }
        }
    }
}
