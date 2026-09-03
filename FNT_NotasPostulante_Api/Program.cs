using FNT_Application;
using FNT_NotasPostulante_Api;
using FNT_NotasPostulante_Api.Endpoints;
using FNT_NotasPostulante_Api.Middleware;
using FNT_Persistence;
using Serilog;

Log.Logger = LogginConfiguration.CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    Log.Logger = LogginConfiguration.AddLoggerConfiguration(builder.Configuration);

    builder.AddLogging();
    builder.AddApiServices();
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);

    var app = builder.Build();

    app.UseApiPipeline();
    app.MapPostulanteNotas();

    Log.Information("Iniciando FNT_NotasPostulante_Api");
    app.Run();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Error al iniciar FNT_NotasPostulante_Api");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
