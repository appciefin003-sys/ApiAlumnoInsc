using System.Text.Json;
using ApiNotasPostulante.Api.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ApiNotasPostulante.Tests;

public class ExceptionMiddlewareTests
{
    [Fact]
    public async Task DevuelveErrorInternoSinExponerLaExcepcion()
    {
        var middleware = new ExceptionMiddleware(
            _ => throw new InvalidOperationException("detalle interno"),
            NullLogger<ExceptionMiddleware>.Instance);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await middleware.InvokeAsync(context);

        context.Response.Body.Position = 0;
        using var response = await JsonDocument.ParseAsync(context.Response.Body);

        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        Assert.False(response.RootElement.GetProperty("isSuccess").GetBoolean());
        Assert.Equal(
            "Ocurrió un error al actualizar las notas",
            response.RootElement.GetProperty("message").GetString());
        Assert.DoesNotContain("detalle interno", response.RootElement.ToString());
    }

    [Fact]
    public async Task PropagaLaCancelacionDeLaSolicitud()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        var middleware = new ExceptionMiddleware(
            _ => throw new OperationCanceledException(cancellation.Token),
            NullLogger<ExceptionMiddleware>.Instance);
        var context = new DefaultHttpContext
        {
            RequestAborted = cancellation.Token
        };

        await Assert.ThrowsAsync<OperationCanceledException>(() => middleware.InvokeAsync(context));
    }
}
