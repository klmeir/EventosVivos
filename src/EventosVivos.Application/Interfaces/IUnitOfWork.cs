namespace EventosVivos.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync(CancellationToken ct);
    }
}
