using ApiNotasPostulante.Application;
using ApiNotasPostulante.Application.Features;
using ApiNotasPostulante.Domain.Postulante;
using ApiNotasPostulante.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ApiNotasPostulante.Tests;

public class DependencyInjectionTests
{
    [Fact]
    public void ResuelveHandlerRepositoryYUnitOfWork()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:UcciConnection"] = "Server=localhost;Database=Test;Integrated Security=True;TrustServerCertificate=True"
            })
            .Build();
        var services = new ServiceCollection();
        services.AddApplicationServices();
        services.AddPersistenceServices(configuration);

        using var provider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateOnBuild = true,
            ValidateScopes = true
        });
        using var scope = provider.CreateScope();

        Assert.NotNull(scope.ServiceProvider.GetRequiredService<ActualizarNotasPostulanteHandler>());
        Assert.NotNull(scope.ServiceProvider.GetRequiredService<IActualizarNotasPostulanteRepository>());
        Assert.NotNull(scope.ServiceProvider.GetRequiredService<IUnitOfWork>());
    }
}
