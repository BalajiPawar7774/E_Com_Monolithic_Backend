using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Com_Monolithic.Dtos
{
    public class ProductDto
    {
        [Required( ErrorMessage = "Seller Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Seller Id must be a positive integer")]
        public int SellerId { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be a positive number")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Product name must be between 3 and 200 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 2000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 1000000, ErrorMessage = "Price must be between 0.01 and 1,000,000")]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Brand name cannot exceed 100 characters")]
        public string? Brand { get; set; }
    }
}
