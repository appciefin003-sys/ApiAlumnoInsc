using ApiNotasPostulante.Domain.Postulante;
using ApiNotasPostulante.Persistence.Contexts;
using ApiNotasPostulante.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiNotasPostulante.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UcciDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("UcciConnection")));

        services.AddScoped<IActualizarNotasPostulanteRepository, ActualizarNotasPostulanteRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
