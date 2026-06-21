using EventosVivos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventosVivos.Api.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication>
            InitializeDatabaseAsync(
                this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var logger =
                services.GetRequiredService<
                    ILoggerFactory>()
                .CreateLogger("DatabaseInitialization");

            try
            {
                var db =
                    services.GetRequiredService<EventosVivosDbContext>();

                logger.LogInformation(
                    "Applying database migrations");

                await db.Database.MigrateAsync();

                logger.LogInformation(
                    "Running database seed");

                var seeder =
                    services.GetRequiredService<ApplicationDbSeeder>();

                await seeder.SeedAsync();

                logger.LogInformation(
                    "Database initialization completed");
            }
            catch (Exception ex)
            {
                logger.LogCritical(
                    ex,
                    "Database initialization failed");

                throw;
            }

            return app;
        }
    }
}
