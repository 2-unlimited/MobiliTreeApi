using MobiliTreeApi.Events.Models;

namespace MobiliTreeApi.Events;

public class SessionCreated(Session session, EventContext context) : DomainEvent(context)
{
    public string SessionId { get; set; } = $"{session.ParkingFacilityId}-{session.CustomerId}-{session.StartDateTime:yyyyMMddHHmmss}";
    public string ParkingFacilityId { get; set; } = session.ParkingFacilityId;
    public string CustomerId { get; set; } = session.CustomerId;
    public DateTime StartDateTime { get; set; } = session.StartDateTime;
    public DateTime? EndDateTime { get; set; } = session.EndDateTime;
}
