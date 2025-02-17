using Microsoft.AspNetCore.Identity;

namespace AspNetCoreOpeniddictPlus.Identity.Entities;

public class OpeniddictPlusRole : IdentityRole
{
    public ICollection<OpeniddictPlusRolePermission> RolePermissions { get; set; } = [];
    
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
}
