namespace AspNetCoreOpeniddictPlus.Core.Dtos;

public sealed class PermissionDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string PermissionId { get; set; } = string.Empty;
}