using System.Collections.Generic;
using MobiliTreeApi.Domain;

namespace MobiliTreeApi.Repositories
{
    public class ParkingFacilityRepositoryFake : IParkingFacilityRepository
    {
        private readonly Dictionary<string, ServiceProfile> _serviceProfiles;

        public ParkingFacilityRepositoryFake(Dictionary<string, ServiceProfile> serviceProfiles)
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
}
