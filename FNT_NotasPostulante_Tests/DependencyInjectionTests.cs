using FNT_Application;
using FNT_Application.Features;
using FNT_Domain;
using FNT_Domain.PostulanteAggregates.Interface;
using FNT_Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FNT_NotasPostulante_Tests;

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
        services.AddInfrastructureServices(configuration);

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
