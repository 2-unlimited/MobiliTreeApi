using MobiliTreeApi.Events;
using MobiliTreeApi.Events.Models;

namespace MobiliTreeApi.Shared.Projection;

public partial class SessionProjection
{
    public void AddSessionFromEvent(SessionCreated sessionCreatedEvent)
    {
        var session = new Session
        {
            ParkingFacilityId = sessionCreatedEvent.ParkingFacilityId,
            CustomerId = sessionCreatedEvent.CustomerId,
            StartDateTime = sessionCreatedEvent.StartDateTime,
            EndDateTime = sessionCreatedEvent.EndDateTime ?? System.DateTime.MinValue
        };

        storage.UpsertSession(session);
    }
}