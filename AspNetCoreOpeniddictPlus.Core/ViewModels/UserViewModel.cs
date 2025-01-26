using System.ComponentModel.DataAnnotations;

namespace AspNetCoreOpeniddictPlus.Core.ViewModels;

public sealed class UserViewModel
{
        
        [Required(ErrorMessage = "UserName is required")]
        [Display(Name = "Username")]
        [StringLength(100, ErrorMessage = "User name cannot exceed 100 characters")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
}