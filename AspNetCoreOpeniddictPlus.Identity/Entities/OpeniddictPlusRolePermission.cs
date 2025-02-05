namespace AspNetCoreOpeniddictPlus.Identity.Entities;

public class OpeniddictPlusRolePermission
{
    public string RoleId { get; set; } = string.Empty;
    public OpeniddictPlusRole Role { get; set; } = null!;

    public Guid PermissionId { get; set; }
    public OpeniddictPlusPermission Permission { get; set; } = null!;
}