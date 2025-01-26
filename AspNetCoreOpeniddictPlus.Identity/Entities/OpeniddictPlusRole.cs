using Microsoft.AspNetCore.Identity;

namespace AspNetCoreOpeniddictPlus.Identity.Entities;

public class OpeniddictPlusRole : IdentityRole
{
    public ICollection<OpeniddictPlusPermission> Permissions { get; set; } = [];
}
