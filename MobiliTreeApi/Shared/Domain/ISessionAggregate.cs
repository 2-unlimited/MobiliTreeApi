namespace MobiliTreeApi.Shared.Domain;

/// <summary>
/// Event-sourced aggregate for parking sessions.
/// Maintains session state through event replay.
/// </summary>
public partial interface ISessionAggregate
{
    string AggregateId { get; }
}