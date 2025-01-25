using AspNetCoreOpeniddictPlus.Core.Dtos;
using AspNetCoreOpeniddictPlus.Core.Extensions;
using AspNetCoreOpeniddictPlus.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreOpeniddictPlus.Core.Services;

public class RoleService<TEntity, TDbContext>(TDbContext dbContext) : IRoleService<TEntity>
    where TEntity : class
    where TDbContext : DbContext
{
    
    public async Task<PagedResult<TEntity>> GetRolesAsync(int page = 1, int pageSize = 10, string? orderBy = null)
    {
        var query = dbContext.Set<TEntity>().AsQueryable();
        if (query is null)
        {
            throw new InvalidOperationException(
                "The provided entity type is not a valid queryable type."
            );
        }
        if (orderBy is not null)
        {
            query = query.OrderBy(e => EF.Property<object>(e, orderBy));
        }
        return await query.ToPagedResultAsync(page, pageSize);
    }
}
