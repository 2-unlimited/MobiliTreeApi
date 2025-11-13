using System.Collections.Generic;
using MobiliTreeApi.Events.Models;

namespace MobiliTreeApi.Shared.Storage;

/// <summary>
/// Repository layer that accesses the fake storage.
/// </summary>
public class SessionStorage(IFakeBaseSessionStorage fakeStorage) : ISessionStorage
{
    /// <summary>
    /// Adds or updates a session in the read model.
    /// </summary>
    public void UpsertSession(Session session)
    {
        var sessionId = $"{session.ParkingFacilityId}-{session.CustomerId}-{session.StartDateTime:yyyyMMddHHmmss}";

        fakeStorage.Upsert(sessionId, session);
    }

    /// <summary>
    /// Gets a session by its ID.
    /// </summary>
    public Session GetSessionById(string sessionId)
    {
        return fakeStorage.Read(sessionId);
    }

    /// <summary>
    /// Gets all sessions for a specific parking facility.
    /// </summary>
    public List<Session> GetSessionsByFacility(string parkingFacilityId)
    {
        return fakeStorage.Query(s => s.ParkingFacilityId == parkingFacilityId);
    }

    /// <summary>
    /// Gets all sessions across all facilities.
    /// </summary>
    public List<Session> GetAllSessions()
    {
        return fakeStorage.GetAll();
    }
}
