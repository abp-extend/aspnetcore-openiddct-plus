using System.ComponentModel.DataAnnotations;

namespace AspNetCoreOpeniddictPlus.Core.ViewModels
{
    public sealed class RoleViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        [Display(Name = "Role Name")]
        [StringLength(100, ErrorMessage = "Role name cannot exceed 100 characters")]
        public string Name { get; set; }
    }
}
