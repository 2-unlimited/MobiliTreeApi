using System.Collections.Generic;
using MobiliTreeApi.Events.Models;

namespace MobiliTreeApi.Shared.Projection;

public partial interface ISessionProjection
{
    List<Session> GetSessions(string parkingFacilityId);
}
