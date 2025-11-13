using System;
using MobiliTreeApi.Events.Models;
using MobiliTreeApi.Shared.Storage;

namespace MobiliTreeApi.CreateSession.Domain;

public class SessionManagement(ISessionAggregateRepository aggregateRepository) : ISessionManagement
{
    /// <inheritdoc />
    public void CreateSession(Session session)
    {
        var sessionId = $"{session.ParkingFacilityId}-{session.CustomerId}-{session.StartDateTime:yyyyMMddHHmmss}";

        var aggregate = aggregateRepository.LoadOrCreateSessionAggregate(sessionId);

        var eventContext = new EventContext(
            DateTimeOffset.UtcNow,
            UserId: session.CustomerId,
            SourceSystem: "MobiliTreeApi",
            CorrelationId: Guid.NewGuid().ToString()
        );

        var result = aggregate.CreateSession(session, eventContext);

        if (result.UnitOfWorkShouldComplete)
        {
            aggregateRepository.SaveChanges();
        }
    }
}
