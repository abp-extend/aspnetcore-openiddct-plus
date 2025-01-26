namespace AspNetCoreOpeniddictPlus.Identity.Entities;

public class OpeniddictPlusPermission
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    
    public string RoleId { get; set; } = string.Empty;
    
}
