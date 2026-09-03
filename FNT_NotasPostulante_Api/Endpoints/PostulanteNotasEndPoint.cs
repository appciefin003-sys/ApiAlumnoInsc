using FNT_Application.DTOs;
using FNT_Application.Features;
using FNT_CrossCutting;

namespace FNT_NotasPostulante_Api.Endpoints;

public static class PostulanteNotasEndPoint
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
        .Produces<Response<bool>>(StatusCodes.Status200OK)
        .Produces<Response<bool>>(StatusCodes.Status400BadRequest)
        .Produces<Response<bool>>(StatusCodes.Status500InternalServerError)
        .WithOpenApi();

        return api;
    }
}
