namespace AspNetCoreOpeniddictPlus.Core.Dtos;

public sealed class RolePermissionDto
{
    public string RoleId { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public List<PermissionDto> Permissions { get; set; } = [];
}