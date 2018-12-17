using System.Collections.Generic;
using MobiliTreeApi.Domain;

namespace MobiliTreeApi.Repositories
{
    public interface ISessionsRepository
    {
        void AddSession(Session session);
        List<Session> GetSessions(string parkingFacilityId);
    }
}