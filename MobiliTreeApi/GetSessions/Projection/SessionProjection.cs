using System.Collections.Generic;
using MobiliTreeApi.Events.Models;

namespace MobiliTreeApi.Shared.Projection;

public partial class SessionProjection
{
    public List<Session> GetSessions(string parkingFacilityId)
    {
        return storage.GetSessionsByFacility(parkingFacilityId);
    }
}
