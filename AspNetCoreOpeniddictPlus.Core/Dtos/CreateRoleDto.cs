namespace AspNetCoreOpeniddictPlus.Core.Dtos;

public class CreateRoleDto
{
    public string RoleName { get; set; } = null!;
    public string? Description { get; set; }
    public Guid? PermissionId { get; set; }
}