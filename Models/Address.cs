using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace E_Com_Monolithic.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [StringLength(100)]
        public string AddressLine1 { get; set; } = string.Empty;

        [StringLength(100)]
        public string AddressLine2 { get; set; } = string.Empty;

        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [StringLength(100)]
        public string State { get; set; } = string.Empty;

        [StringLength(100)]
        public string Country { get; set; } = string.Empty;

        [StringLength(10)]
        public string Pincode { get; set; } = string.Empty;

        public bool IsDefault { get; set; } = false;

    }
}
