namespace EventosVivos.Application.DTOs
{
    public record EventReportDto(
        int TotalTicketsSold,
        int TotalTicketsAvailable,
        decimal OccupancyPercentage,
        decimal TotalRevenue,
        string Status
    );
}
