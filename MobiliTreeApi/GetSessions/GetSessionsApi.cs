using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MobiliTreeApi.Shared.Projection;

namespace MobiliTreeApi.GetSessions;

public static class GetSessionsApi
{
    /// <summary>
    /// Maps the GetSessions API endpoint.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    public static void MapGetSessionsApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/sessions/{parkingFacilityId}", GetSessions)
            .WithName(nameof(GetSessions));
    }

    /// <summary>
    /// Gets all sessions for a specific parking facility.
    /// </summary>
    /// <param name="sessionProjection">The projection containing the sessions.</param>
    /// <param name="parkingFacilityId">The parking facility ID.</param>
    /// <returns>List of sessions for the facility.</returns>
    private static IResult GetSessions(
        [FromServices] ISessionProjection sessionProjection,
        [FromRoute] string parkingFacilityId)
    {
        var sessions = sessionProjection.GetSessions(parkingFacilityId);
        return Results.Ok(sessions);
    }
}
