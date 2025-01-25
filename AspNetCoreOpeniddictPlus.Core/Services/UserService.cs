using AspNetCoreOpeniddictPlus.Core.Dtos;
using AspNetCoreOpeniddictPlus.Core.Extensions;
using AspNetCoreOpeniddictPlus.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreOpeniddictPlus.Core.Services;

public class UserService<TEntity, TDbContext>(TDbContext dbContext) : IUserService<TEntity>
    where TEntity : class
    where TDbContext : DbContext
{
    public async Task<PagedResult<TEntity>> GetUsersAsync(
        int page,
        int pageSize,
        string? orderBy = null
    )
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
