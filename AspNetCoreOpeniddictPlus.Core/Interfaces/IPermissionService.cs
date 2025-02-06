
namespace AspNetCoreOpeniddictPlus.Core.Interfaces;

public interface IPermissionService<PEntity>
{
    Task<IQueryable<PEntity>> GetPermissions();
    
    Task<PEntity?> GetPermissionByNameAsync(string permissionName);
    
    Task<PEntity> GetPermissionByIdAsync(string permissionId);
    
    Task CreatePermissionAsync(PEntity entity);
    
    Task UpdatePermissionAsync(string permissionId, PEntity entity);
    
    Task DeletePermissionAsync(string permissionId);
}