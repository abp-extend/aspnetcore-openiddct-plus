using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreOpeniddictPlus.Core.Services;

public class PermissionRequirement<T>(T permission) : IAuthorizationRequirement
{
    public T Permission { get; } = permission;
}
