using AspNetCoreOpeniddictPlus.Core.Dtos;

namespace AspNetCoreOpeniddictPlus.Core.Interfaces;

public interface IPermissionService<PEntity>
{
    Task<PagedResult<PEntity>> GetRolesAsync(
        int page = 1,
        int pageSize = 10,
        string? orderBy = null
    );
    
    Task<PEntity?> GetPermissionByNameAsync(string permissionName);
    
    Task<PEntity> GetPermissionByIdAsync(string permissionId);
    
    Task CreatePermissionAsync(PEntity entity);
    
    Task UpdatePermissionAsync(string permissionId, PEntity entity);
    
    Task DeletePermissionAsync(string permissionId);
}