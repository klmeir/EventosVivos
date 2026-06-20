using EventosVivos.Application.Behaviors;
using EventosVivos.Application.Mappings;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EventosVivos.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(
                    typeof(DependencyInjection).Assembly);
            });

            services.AddValidatorsFromAssembly(
                typeof(DependencyInjection).Assembly);

            services.AddAutoMapper(cfg => {
                cfg.AddProfile<MappingProfile>();
            }, typeof(MappingProfile).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));         

            return services;
        }
    }
}
