namespace AspNetCoreOpeniddictPlus.Identity.Entities;

public class OpeniddictPlusPermission
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    
    public ICollection<OpeniddictPlusRolePermission> RolePermissions { get; set; } = [];
    
}
