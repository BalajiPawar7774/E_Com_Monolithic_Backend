using System.ComponentModel.DataAnnotations;

namespace E_Com_Monolithic.Dtos
{
    public class CategoryDto
    {
        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Category Name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty;
    }
}
