using System.ComponentModel.DataAnnotations;
namespace E_Com_Monolithic.Dtos
{
    public class UserDto
    {
        [Required (ErrorMessage = "Full name is required")]
        public string FullName { get; set; } = string.Empty;
        [Required (ErrorMessage = "Email is required")]
        [EmailAddress (ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;
        [Required (ErrorMessage = "Phone number is required")]
        [Phone (ErrorMessage = "Invalid phone number format")]
        public string Phone { get; set; } = string.Empty;
        [Required (ErrorMessage = "Password is required")]
        public string PasswordHash { get; set; } = string.Empty;
        [Required (ErrorMessage = "Role is required")]
        public string Role { get; set; } = string.Empty;
    }
}
