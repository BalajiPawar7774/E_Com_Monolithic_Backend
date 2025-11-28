using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace E_Com_Monolithic.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Address")]
        public int AddressId { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public Decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Order Created";
        public DateTime CreatedAt { get; set; }
        public Order()
        {
            CreatedAt = DateTime.UtcNow;
        }

    }
}
