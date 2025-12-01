using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace E_Com_Monolithic.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime PaidAt { get; set; }
        public string TransactionId { get; set; } = string.Empty;

        public Payment()
        {
            PaidAt = DateTime.UtcNow;
        }

    }
}
