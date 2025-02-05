using AspNetCoreOpeniddictPlus.Core.Dtos;
using AspNetCoreOpeniddictPlus.Core.Extensions;
using AspNetCoreOpeniddictPlus.Core.Interfaces;
using AspNetCoreOpeniddictPlus.Identity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreOpeniddictPlus.Web.Controllers;

public class PermissionWithRoleAssignedDto
{
    public string PermissionId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> RoleIds { get; set; } = [];
}

[ApiController]
[Route("api/[controller]")]
public class PermissionController(IPermissionService<OpeniddictPlusPermission> permissionService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> ListAll(int currentPage = 1, int pageSize = 10)
    {

        var permissions = await permissionService.GetPermissions();
        var pagedResult = await permissions.Include(r => r.RolePermissions)
            .ToPagedResultAsync(currentPage, pageSize);
            
        var convertedPermissionDtos = permissions.Select(p => new PermissionWithRoleAssignedDto
        {
            PermissionId = p.Id.ToString(),
            Name = p.Name,
            Description = p.Description,
            RoleIds = p.RolePermissions.Select(rp => rp.RoleId.ToString()).ToList()
        }).ToList();


        var result = new PagedResult<PermissionWithRoleAssignedDto>
        {
            Items = convertedPermissionDtos,
            TotalCount = pagedResult.TotalCount,
            CurrentPage = pagedResult.CurrentPage,
            PageSize = pagedResult.PageSize,
        };
        return Ok(result);
    }
}
