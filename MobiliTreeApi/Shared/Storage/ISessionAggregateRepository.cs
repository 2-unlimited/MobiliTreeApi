using System.Threading.Tasks;
using MobiliTreeApi.Shared.Domain;

namespace MobiliTreeApi.Shared.Storage;

/// <summary>
/// Repository for event-sourced session aggregates.
/// In a real implementation, this would persist events to an event store.
/// </summary>
public interface ISessionAggregateRepository
{
    Task<SessionAggregate> LoadOrCreateSessionAggregateAsync(string sessionId);
    SessionAggregate LoadOrCreateSessionAggregate(string sessionId);
    Task SaveChangesAsync();
    void SaveChanges();
}