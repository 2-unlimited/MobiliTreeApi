namespace MobiliTreeApi.Events.Models;

/// <summary>
/// Context information for events.
/// </summary>
public record EventContext(DateTimeOffset Timestamp, string UserId, string SourceSystem, string? CorrelationId = null);