using System;
using System.Collections.Generic;
using System.Linq;
using MobiliTreeApi.GetInvoices.Storage;
using MobiliTreeApi.Shared.Domain;
using MobiliTreeApi.Shared.Projection;

namespace MobiliTreeApi.GetInvoices.Domain;

public class InvoiceManagement : IInvoiceManagement
{
    private readonly ISessionProjection _sessionProjection;
    private readonly IParkingFacilityRepository _parkingFacilityRepository;

    public InvoiceManagement(
        ISessionProjection sessionProjection,
        IParkingFacilityRepository parkingFacilityRepository)
    {
        _sessionProjection = sessionProjection;
        _parkingFacilityRepository = parkingFacilityRepository;
    }

    /// <inheritdoc />
    public List<Invoice> GetInvoices(string parkingFacilityId)
    {
        var serviceProfile = _parkingFacilityRepository.GetServiceProfile(parkingFacilityId);
        if (serviceProfile == null)
        {
            throw new ArgumentException($"Invalid parking facility id '{parkingFacilityId}'");
        }

        var sessions = _sessionProjection.GetSessions(parkingFacilityId);

        return sessions.GroupBy(x => x.CustomerId).Select(x => new Invoice
        {
            ParkingFacilityId = parkingFacilityId,
            CustomerId = x.Key,
            Amount = 0
        }).ToList();
    }
}
