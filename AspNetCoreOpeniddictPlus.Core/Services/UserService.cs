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

    public async Task<TEntity> GetUserByIdAsync(string userId)
    {
        var query = dbContext.Set<TEntity>().AsQueryable();
        if (query is null)
        {
            throw new InvalidOperationException(
                "The provided entity type is not a valid queryable type."
            );
        }
        var user = await query.FirstOrDefaultAsync(r => EF.Property<object>(r, "Id").Equals(userId));
        if (user is null)
        {
            throw new InvalidOperationException(
                "The provided user id is not valid."
            );
        }
        
        return user;
    }

    public  async Task CreateUserAsync(TEntity entity)
    {
        var query = dbContext.Set<TEntity>().AsQueryable();
        if (query is null)
        {
            throw new InvalidOperationException(
                "The provided entity type is not a valid queryable type."
            );
        }
        try
        {
            await dbContext.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to process this operation. Message: {e.Message}");
        }
        
    }

    public async Task UpdateUserAsync(string userId, TEntity entity)
    {
        var query = dbContext.Set<TEntity>().AsQueryable();
        if (query is null)
        {
            throw new InvalidOperationException(
                "The provided entity type is not a valid queryable type."
            );
        }
        var role = await query.Where(r => EF.Property<object>(r, "Id").Equals(userId)).FirstOrDefaultAsync();
        if (role is null)
        {
            throw new InvalidOperationException(
                "The provided role id is not valid."
            );
        }
        dbContext.Update(entity); 
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(string userId)
    {
        var query = dbContext.Set<TEntity>().AsQueryable();
        if (query is null)
        {
            throw new InvalidOperationException(
                "The provided entity type is not a valid queryable type."
            );
        }
        var role = await query.Where(r => EF.Property<object>(r, "Id").Equals(userId)).FirstOrDefaultAsync();
        if (role is null)
        {
            throw new InvalidOperationException(
                "The provided role id is not valid."
            );
        }
        dbContext.Remove(role);
        await dbContext.SaveChangesAsync();
    }
}
