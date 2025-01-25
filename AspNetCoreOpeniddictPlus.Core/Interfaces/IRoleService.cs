using AspNetCoreOpeniddictPlus.Core.Dtos;

namespace AspNetCoreOpeniddictPlus.Core.Interfaces;

public interface IRoleService<TEntity>
{
    Task<PagedResult<TEntity>> GetRolesAsync(
        int page = 1,
        int pageSize = 10,
        string? orderBy = null
    );
    
    Task<TEntity> GetRoleByIdAsync(Guid roleId);
    
    Task<Guid> CreateRoleAsync(CreateRoleDto roleDto);
    
    Task<Guid> UpdateRoleAsync(Guid roleId, UpdateRoleDto updateRoleDto);
    
    Task<Guid> DeleteRoleAsync(Guid roleId);
}
