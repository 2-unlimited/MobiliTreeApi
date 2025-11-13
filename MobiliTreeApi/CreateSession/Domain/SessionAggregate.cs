using System;
using MobiliTreeApi.Events;
using MobiliTreeApi.Events.Models;
using MobiliTreeApi.Shared.Results;

namespace MobiliTreeApi.Shared.Domain;

public partial class SessionAggregate
{
    private DateTime? _startDateTime;

    /// <summary>
    /// Creates a new session if it doesn't already exist.
    /// </summary>
    public ICommandResult CreateSession(Session session, EventContext eventContext)
    {
        // Idempotency check
        if (_startDateTime.HasValue)
        {
            return ICommandResult.WasAlreadyDone;
        }

        var sessionCreatedEvent = new SessionCreated(session, eventContext);
        Append(sessionCreatedEvent);

        return ICommandResult.Done;
    }

    /// <summary>
    /// Applies SessionCreated event to aggregate state.
    /// </summary>
    private void Apply(SessionCreated @event)
    {
        _sessionId = @event.SessionId;
        _startDateTime = @event.StartDateTime;
    }
}
