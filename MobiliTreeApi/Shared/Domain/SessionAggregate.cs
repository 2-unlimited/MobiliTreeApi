using System.Collections.Generic;
using MobiliTreeApi.Events.Models;

namespace MobiliTreeApi.Shared.Domain;

public partial class SessionAggregate(string sessionId = null) : ISessionAggregate
{
    private readonly List<DomainEvent> _uncommittedEvents = [];
    private string _sessionId = sessionId;

    public string AggregateId => _sessionId;

    /// <summary>
    /// Appends an event and applies it to the aggregate state.
    /// </summary>
    private void Append(DomainEvent @event)
    {
        _uncommittedEvents.Add(@event);
        Apply((dynamic)@event);
    }

    /// <summary>
    /// Loads the aggregate from historical events (event sourcing pattern).
    /// </summary>
    public void LoadFromHistory(IEnumerable<DomainEvent> events)
    {
        foreach (var @event in events)
        {
            Apply((dynamic)@event);
        }
    }

    /// <summary>
    /// Gets uncommitted events for persistence.
    /// </summary>
    public IReadOnlyList<DomainEvent> GetUncommittedEvents()
    {
        return _uncommittedEvents.AsReadOnly();
    }

    /// <summary>
    /// Clears uncommitted events after persistence.
    /// </summary>
    public void MarkEventsAsCommitted()
    {
        _uncommittedEvents.Clear();
    }
}
