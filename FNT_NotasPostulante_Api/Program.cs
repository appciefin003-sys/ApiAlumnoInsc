using FNT_Application;
using FNT_NotasPostulante_Api;
using FNT_NotasPostulante_Api.Endpoints;
using FNT_Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.UseApiPipeline();
app.MapPostulanteNotas();

app.Run();
