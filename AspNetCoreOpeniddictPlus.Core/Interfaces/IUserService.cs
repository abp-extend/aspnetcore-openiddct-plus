using AspNetCoreOpeniddictPlus.Core.Dtos;

namespace AspNetCoreOpeniddictPlus.Core.Interfaces;

public interface IUserService<TEntity>
{
    Task<PagedResult<TEntity>> GetUsersAsync(
        int page = 1,
        int pageSize = 10,
        string? orderBy = null
    );
    
    Task<TEntity> GetUserByIdAsync(string userId);
    
    Task CreateUserAsync(TEntity entity);
    
    Task UpdateUserAsync(string userId, TEntity entity);
    
    Task DeleteUserAsync(string userId);
    
}
