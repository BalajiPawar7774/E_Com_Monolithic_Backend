using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace E_Com_Monolithic.Models
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Cart()
        {
            CreatedAt = DateTime.UtcNow;
        }   
    }
}
