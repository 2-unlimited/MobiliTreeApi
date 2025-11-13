using System.Collections.Generic;
using MobiliTreeApi.Events.Models;
using MobiliTreeApi.Shared.Domain;

namespace MobiliTreeApi.Shared.Storage;

/// <summary>
/// Generic fake in-memory repository for the session read model.
/// This is a simple data store with no business logic - just CRUD operations.
/// In production, this would be a database table optimized for queries.
/// The SessionProjection contains the business logic for populating this repository from events.
/// </summary>
public interface ISessionStorage
{
    void UpsertSession(Session session);
    Session GetSessionById(string sessionId);
    List<Session> GetSessionsByFacility(string parkingFacilityId);
    List<Session> GetAllSessions();
}