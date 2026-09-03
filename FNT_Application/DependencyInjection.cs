using FNT_Application.Features;
using FNT_Application.Validators;
using FNT_Domain.PostulanteAggregates;
using Microsoft.Extensions.DependencyInjection;

namespace FNT_Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ActualizarNotasPostulanteHandler>();
        services.AddScoped<ActualizarNotasPostulanteValidator>();
        services.AddScoped<NotasPostulanteService>();

        return services;
    }
}
