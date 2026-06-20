namespace EventosVivos.Application.DTOs
{
    public record EventDto(
        Guid Id,
        string Title,
        string Description,
        Guid VenueId,
        int MaxCapacity,
        int AvailableTickets,
        DateTime StartTime,
        DateTime EndTime,
        decimal Price,
        string EventType,
        string Status
    );
}
