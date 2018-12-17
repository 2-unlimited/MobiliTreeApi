namespace MobiliTreeApi.Domain
{
    public class Invoice
    {
        public string ParkingFacilityId { get; set; }
        public string CustomerId { get; set; }
        public decimal Amount { get; set; }
    }
}