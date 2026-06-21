using EventosVivos.Application.Interfaces;
using EventosVivos.Domain.Ports;
using EventosVivos.Domain.Services;
using EventosVivos.Infrastructure.Data;
using EventosVivos.Infrastructure.Repositories;
using EventosVivos.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventosVivos.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<EventosVivosDbContext>(options =>
            {
                options.UseSqlite(
                    configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IVenueRepository, VenueRepository>();
            services.AddScoped<VenueAvailabilityChecker>();

            services.AddScoped<ApplicationDbSeeder>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
