using MobiliTreeApi.Shared.Domain;

namespace MobiliTreeApi.GetInvoices.Storage;

public interface IParkingFacilityRepository
{
    ServiceProfile GetServiceProfile(string parkingFacilityId);
}
