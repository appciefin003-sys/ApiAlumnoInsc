using ApiNotasPostulante.Application.DTOs;
using ApiNotasPostulante.Application.Features;

namespace ApiNotasPostulante.Api.Endpoints;

public static class PostulanteNotasEndpoint
{
    public static RouteGroupBuilder MapPostulanteNotas(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/v1/postulante-notas").WithTags("Actualizar Notas Postulante");

        api.MapPost("/", async (
            ActualizarNotasPostulanteRequest request,
            ActualizarNotasPostulanteHandler handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler.EjecutarAsync(request, cancellationToken);
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        })
        .WithName("EjecutarActualizarNotasPostulante")
        .WithOpenApi();

        return api;
    }
}
