namespace MobiliTreeApi.Events.Models;

/// <summary>
/// Base class for all domain events.
/// </summary>
public abstract class DomainEvent(EventContext context)
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
    public EventContext Context { get; set; } = context;
}