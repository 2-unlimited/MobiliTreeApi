using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MobiliTreeApi.CreateSession.Domain;
using MobiliTreeApi.Events.Models;

namespace MobiliTreeApi.CreateSession;

public static class CreateSessionApi
{
    /// <summary>
    /// Maps the CreateSession API endpoint.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    public static void MapCreateSessionApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/sessions", CreateSession)
            .WithName(nameof(CreateSession));
    }

    /// <summary>
    /// Creates a new parking session.
    /// </summary>
    /// <param name="sessionManagement">The session management service.</param>
    /// <param name="session">The session to create.</param>
    /// <returns>Ok if successful.</returns>
    private static IResult CreateSession(
        [FromServices] ISessionManagement sessionManagement,
        [FromBody] Session session)
    {
        sessionManagement.CreateSession(session);
        return Results.Ok();
    }
}
