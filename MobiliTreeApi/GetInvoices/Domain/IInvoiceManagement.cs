using System.Collections.Generic;
using MobiliTreeApi.Shared.Domain;

namespace MobiliTreeApi.GetInvoices.Domain;

public interface IInvoiceManagement
{
    /// <summary>
    /// Gets invoices for a specific parking facility.
    /// </summary>
    /// <param name="parkingFacilityId">The parking facility ID.</param>
    /// <returns>List of invoices.</returns>
    List<Invoice> GetInvoices(string parkingFacilityId);
}
