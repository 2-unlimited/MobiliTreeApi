using System.Collections.Generic;
using System.Linq;
using MobiliTreeApi.Domain;

namespace MobiliTreeApi.Repositories
{
    public class SessionsRepositoryFake : ISessionsRepository
    {
        private readonly List<Session> _sessions;

        public SessionsRepositoryFake(List<Session> sessions)
        {
            _sessions = sessions;
        }

        public void AddSession(Session session)
        {
            _sessions.Add(session);
        }

        public List<Session> GetSessions(string parkingFacilityId)
        {
            return _sessions.Where(x => x.ParkingFacilityId == parkingFacilityId).ToList();
        }
    }
}
