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
            if (await _context.Venues.AnyAsync(ct) && await _context.Events.AnyAsync(ct))
            {
                _logger.LogInformation(
                    "Database already seeded");

                return;
            }

            if (!await _context.Venues.AnyAsync(ct))
            {
                var venues = CreateVenues();
                await _context.Venues.AddRangeAsync(venues, ct);
                _logger.LogInformation("Seeded {Count} venues", venues.Count);
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

        private static List<Venue> CreateVenues()
        {
            return new List<Venue>
            {
                new(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Auditorio Central", 200, "Bogotá"),
                new(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Sala Norte", 50, "Bogotá"),
                new(Guid.Parse("33333333-3333-3333-3333-333333333333"), "Arena Sur", 500, "Medellín")
            };
        }

        private static List<Event> CreateEvents()
        {
            return new List<Event> { };
        }
    }
}