using System.Collections.Generic;
using MobiliTreeApi.Shared.Domain;

namespace MobiliTreeApi.GetInvoices.Storage;

public class ParkingFacilityRepository : IParkingFacilityRepository
{
    private readonly Dictionary<string, ServiceProfile> _serviceProfiles;

    public ParkingFacilityRepository(Dictionary<string, ServiceProfile> serviceProfiles)
    {
        _serviceProfiles = serviceProfiles;
    }

    public ServiceProfile GetServiceProfile(string parkingFacilityId)
    {
        if (_serviceProfiles.TryGetValue(parkingFacilityId, out var serviceProfile))
        {
            return serviceProfile;    
        }

        return null;
    }
}
