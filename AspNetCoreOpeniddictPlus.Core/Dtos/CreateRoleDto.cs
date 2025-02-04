namespace AspNetCoreOpeniddictPlus.Core.Dtos;

public class CreateRoleDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Guid? PermissionId { get; set; }
}