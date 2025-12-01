using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace E_Com_Monolithic.Models
{
    public class Shipment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ShipmentId { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public string CourierName { get; set; } = string.Empty;
        public string TrackingNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ShippedDate { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public DateTime? DeliveredDate { get; set; }

        public Shipment()
        {
            CreatedAt = DateTime.UtcNow;
            EstimatedDeliveryDate = CreatedAt.AddDays(7); // Default estimated delivery in 7 days
        }

    }
}
