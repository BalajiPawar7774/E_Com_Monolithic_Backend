using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace E_Com_Monolithic.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
