using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace E_Com_Monolithic.Models
{
    public class ProductImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImageId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
