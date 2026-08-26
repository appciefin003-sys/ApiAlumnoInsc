using FNT_NotasPostulante_Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FNT_NotasPostulante_Tests;

public class CorsConfigurationTests
{
    [Fact]
    public async Task PermiteOrigenConfigurado()
    {
        const string origin = "https://notas.example.edu.pe";
        var result = await EvaluarOrigenAsync(origin, origin);

        Assert.True(result.IsOriginAllowed);
    }

    [Fact]
    public async Task RechazaOrigenNoConfigurado()
    {
        var result = await EvaluarOrigenAsync(
            "https://otro.example.edu.pe",
            "https://notas.example.edu.pe");

        Assert.False(result.IsOriginAllowed);
    }

    [Fact]
    public async Task RechazaOrigenCuandoNoHayConfiguracion()
    {
        var result = await EvaluarOrigenAsync("https://notas.example.edu.pe");

        Assert.False(result.IsOriginAllowed);
    }

    private static async Task<CorsResult> EvaluarOrigenAsync(string origin, string? allowedOrigin = null)
    {
        var builder = WebApplication.CreateBuilder();
        if (allowedOrigin is not null)
        {
            builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Cors:AllowedOrigins:0"] = allowedOrigin
            });
        }

        builder.AddApiServices();

        await using var provider = builder.Services.BuildServiceProvider();
        var policyProvider = provider.GetRequiredService<ICorsPolicyProvider>();
        var corsService = provider.GetRequiredService<ICorsService>();
        var context = new DefaultHttpContext();
        context.Request.Headers.Origin = origin;
        var policy = await policyProvider.GetPolicyAsync(context, DependencyInjection.CorsPolicyName);

        return corsService.EvaluatePolicy(context, Assert.IsType<CorsPolicy>(policy));
    }
}
