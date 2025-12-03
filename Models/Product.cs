using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace E_Com_Monolithic.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [ForeignKey("User")]
        public int SellerId { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Brand { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Status { get; set; } = "Available";

        public Product()
        {
            CreatedAt = DateTime.Now;
        }



    }
}
