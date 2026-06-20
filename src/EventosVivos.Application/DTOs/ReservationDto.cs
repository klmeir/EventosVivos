namespace EventosVivos.Application.DTOs
{
    public record ReservationDto(
        Guid Id,
        Guid EventId,
        int Quantity,
        string BuyerName,
        string BuyerEmail,
        string Status, // Se envía como string (ej: "PendingPayment")
        DateTime CreatedAt,
        string? ReservationCode // Nulo si está pendiente
    );
}
