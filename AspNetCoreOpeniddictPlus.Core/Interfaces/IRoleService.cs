using AspNetCoreOpeniddictPlus.Core.Dtos;

namespace AspNetCoreOpeniddictPlus.Core.Interfaces;

public interface IRoleService<TEntity>
{
    Task<PagedResult<TEntity>> GetRolesAsync(
        int page = 1,
        int pageSize = 10,
        string? orderBy = null
    );
    
    Task<TEntity> GetRoleByIdAsync(string roleId);
    
    Task CreateRoleAsync(TEntity entity);
    
    Task UpdateRoleAsync(string roleId, TEntity entity);
    
    Task DeleteRoleAsync(string roleId);
}
