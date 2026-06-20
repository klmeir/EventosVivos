using EventosVivos.Application.Interfaces;

namespace EventosVivos.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EventosVivosDbContext _context;

        public UnitOfWork(EventosVivosDbContext context)
        {
            _context = context;
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _context.SaveChangesAsync(ct);
    }
}
