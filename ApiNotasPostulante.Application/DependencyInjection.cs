using ApiNotasPostulante.Application.Features;
using Microsoft.Extensions.DependencyInjection;

namespace ApiNotasPostulante.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ActualizarNotasPostulanteHandler>();

        return services;
    }
}
