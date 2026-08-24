using ApiNotasPostulante.Application.Features;
using ApiNotasPostulante.Application.Validators;
using ApiNotasPostulante.Domain.Postulante;
using Microsoft.Extensions.DependencyInjection;

namespace ApiNotasPostulante.Application;

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
