using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MobiliTreeApi.Events.Models;
using MobiliTreeApi.Shared.Domain;
using MobiliTreeApi.Shared.ServiceBus;

namespace MobiliTreeApi.Shared.Storage;

/// -- It's only here to facilitate the exercise, does not need to be investigated --
public class FakeSessionAggregateRepository(
    IFakeMessageBus messageBus,
    ILogger<FakeSessionAggregateRepository> logger) : ISessionAggregateRepository
{
    // Fake in-memory event store (in production, this would be a real event store)
    private static readonly Dictionary<string, List<DomainEvent>> EventStore = new();
    private readonly List<SessionAggregate> _trackedAggregates = [];

    /// <summary>
    /// Loads an existing aggregate from event history or creates a new one (async).
    /// </summary>
    public Task<SessionAggregate> LoadOrCreateSessionAggregateAsync(string sessionId)
    {
        return Task.FromResult(LoadOrCreateSessionAggregate(sessionId));
    }

    /// <summary>
    /// Loads an existing aggregate from event history or creates a new one (sync).
    /// </summary>
    public SessionAggregate LoadOrCreateSessionAggregate(string sessionId)
    {
        var aggregate = new SessionAggregate(sessionId);

        // Load historical events if they exist
        if (EventStore.TryGetValue(sessionId, out var events))
        {
            aggregate.LoadFromHistory(events);
        }

        _trackedAggregates.Add(aggregate);
        return aggregate;
    }

    /// <summary>
    /// Persists uncommitted events to the event store (async).
    /// </summary>
    public Task SaveChangesAsync()
    {
        SaveChanges();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Persists uncommitted events to the event store (sync).
    /// </summary>
    public void SaveChanges()
    {
        foreach (var aggregate in _trackedAggregates)
        {
            var uncommittedEvents = aggregate.GetUncommittedEvents();
            if (!uncommittedEvents.Any() || EventStore == null) 
                continue;

            var aggregateId = aggregate.AggregateId;
            if (!EventStore.ContainsKey(aggregateId))
            {
                EventStore[aggregateId] = [];
            }

            EventStore[aggregateId].AddRange(uncommittedEvents);

            // Publish events to the fake message bus
            PublishEventsToServiceBus(uncommittedEvents);

            aggregate.MarkEventsAsCommitted();
        }

        _trackedAggregates.Clear();
    }

    /// <summary>
    /// Publishes events to the fake message bus.
    /// In production, this would publish to a Service Bus.
    /// </summary>
    private void PublishEventsToServiceBus(IReadOnlyList<DomainEvent> events)
    {
        foreach (var @event in events)
        {
            logger.LogInformation(
                "Publishing event {EventType} (EventId: {EventId}) to message bus",
                @event.GetType().Name,
                @event.EventId);

            messageBus.Publish(@event);
        }
    }
}
