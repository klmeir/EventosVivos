using EventosVivos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventosVivos.Infrastructure.Data
{
    public sealed class ApplicationDbSeeder
    {
        private readonly EventosVivosDbContext _context;
        private readonly ILogger<ApplicationDbSeeder> _logger;

        public ApplicationDbSeeder(
            EventosVivosDbContext context,
            ILogger<ApplicationDbSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync(
            CancellationToken ct = default)
        {
            if (await _context.Events.AnyAsync(ct))
            {
                _logger.LogInformation(
                    "Database already seeded");

                return;
            }

            var events = CreateEvents();

            await _context.Events.AddRangeAsync(
                events,
                ct);

            await _context.SaveChangesAsync(ct);

            _logger.LogInformation(
                "Seeded {Count} events",
                events.Count);
        }

        private static List<Event> CreateEvents()
        {
            return new List<Event> { };
        }
    }
}