namespace Shipping.Api.Shipments
{
    public class Shipment
    {
        public Guid Id { get; set; }
        public string OrderId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; };
    }
}
