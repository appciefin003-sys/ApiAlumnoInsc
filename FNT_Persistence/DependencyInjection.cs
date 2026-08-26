using FNT_Domain;
using FNT_Domain.PostulanteAggregates.Interface;
using FNT_Persistence.Context;
using FNT_Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FNT_Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UcciDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("UcciConnection")));

        services.AddScoped<IActualizarNotasPostulanteRepository, ActualizarNotasPostulanteRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
