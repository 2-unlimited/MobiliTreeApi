namespace MobiliTreeApi.Events.Models;

public class Session
{
    public required string ParkingFacilityId { get; set; }
    public required string CustomerId { get; set; }
    public required DateTime StartDateTime { get; set; }
    public required DateTime EndDateTime { get; set; }
}
