using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MobiliTreeApi.GetInvoices.Domain;
using MobiliTreeApi.Shared.Domain;
using System.Collections.Generic;

namespace MobiliTreeApi.GetInvoices;

public static class GetInvoicesApi
{
    /// <summary>
    /// Maps the GetInvoices API endpoints.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    public static void MapGetInvoicesApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/invoices/ping", Ping)
            .WithName("PingInvoices");

        endpoints.MapGet("/invoices/{parkingFacilityId}", GetInvoices)
            .WithName("GetInvoices");
    }

    /// <summary>
    /// Health check endpoint.
    /// </summary>
    /// <returns>Pong response.</returns>
    private static IResult Ping()
    {
        return Results.Ok("pong!");
    }

    /// <summary>
    /// Gets all invoices for a specific parking facility.
    /// </summary>
    /// <param name="invoiceManagement"></param>
    /// <param name="parkingFacilityId">The parking facility ID.</param>
    /// <returns>List of invoices for the facility.</returns>
    private static IResult GetInvoices(
        [FromServices] IInvoiceManagement invoiceManagement,
        [FromRoute] string parkingFacilityId)
    {
        var invoices = invoiceManagement.GetInvoices(parkingFacilityId);
        return Results.Ok(invoices);
    }
}
