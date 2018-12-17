using MobiliTreeApi.Domain;

namespace MobiliTreeApi.Repositories
{
    public interface IParkingFacilityRepository
    {
        ServiceProfile GetServiceProfile(string parkingFacilityId);
    }
}