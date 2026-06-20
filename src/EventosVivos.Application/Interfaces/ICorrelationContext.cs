namespace EventosVivos.Application.Interfaces
{
    public interface ICorrelationContext
    {
        string CorrelationId { get; }
    }
}
