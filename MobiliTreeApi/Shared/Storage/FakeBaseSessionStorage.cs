using System.Collections.Generic;
using System.Linq;
using MobiliTreeApi.Events.Models;

namespace MobiliTreeApi.Shared.Storage;

/// <inheritdoc />
public class FakeBaseSessionStorage : IFakeBaseSessionStorage
{
    private readonly Dictionary<string, Session> _documents = new();

    public FakeBaseSessionStorage()
    {
        foreach (var session in FakeData.GetSeedSessions())
        {
            var sessionId = $"{session.ParkingFacilityId}-{session.CustomerId}-{session.StartDateTime:yyyyMMddHHmmss}";
            _documents[sessionId] = session;
        }
    }

    public void Upsert(string id, Session document)
    {
        _documents[id] = document;
    }

    public Session Read(string id)
    {
        _documents.TryGetValue(id, out var document);
        return document;
    }

    public List<Session> Query(System.Func<Session, bool> predicate)
    {
        return _documents.Values
            .Where(predicate)
            .ToList();
    }

    public List<Session> GetAll()
    {
        return _documents.Values.ToList();
    }
}
