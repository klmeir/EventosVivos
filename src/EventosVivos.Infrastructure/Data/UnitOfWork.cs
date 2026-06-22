using EventosVivos.Application.Exceptions;
using EventosVivos.Application.Interfaces;
using EventosVivos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventosVivos.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EventosVivosDbContext _context;

        public UnitOfWork(EventosVivosDbContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync(CancellationToken ct)
        {
            IncrementVersions();
            
            try
            {
                await _context.SaveChangesAsync(ct);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException("This record was updated by another user. Please try again.");
            }
        }

        private void IncrementVersions()
        {
            foreach (var entry in _context.ChangeTracker.Entries<Event>())
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.IncrementVersion();
                }
            }
        }
    }
}
