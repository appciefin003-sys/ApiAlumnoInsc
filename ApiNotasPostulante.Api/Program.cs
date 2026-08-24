using ApiNotasPostulante.Api.Endpoints;
using ApiNotasPostulante.Api.Middleware;
using ApiNotasPostulante.Application;
using ApiNotasPostulante.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseHttpsRedirection();

app.MapPostulanteNotas();

app.Run();
