using GoVilla.Application.Abstractions.Behaviors;
using GoVilla.Domain.Bookings;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace GoVilla.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(m =>
        {
            m.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            //Order mathers
            m.AddOpenBehavior(typeof(LoggingBehavior<,>));
            m.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddTransient<PricingService>();

        return services;
    }
}