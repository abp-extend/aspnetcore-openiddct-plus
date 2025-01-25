using AspNetCoreOpeniddictPlus.Core.Dtos;
using AspNetCoreOpeniddictPlus.Core.Extensions;
using AspNetCoreOpeniddictPlus.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreOpeniddictPlus.Core.Services;

public class RoleService<TREntity, TDbContext>(TDbContext dbContext) : IRoleService<TREntity>
    where TREntity : class
    where TDbContext : DbContext
{
    
    public async Task<PagedResult<TREntity>> GetRolesAsync(int page = 1, int pageSize = 10, string? orderBy = null)
    {
        var query = dbContext.Set<TREntity>().AsQueryable();
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

    public async Task<TREntity> GetRoleByIdAsync(string roleId)
    {
        var query = dbContext.Set<TREntity>().AsQueryable();
        if (query is null)
        {
            throw new InvalidOperationException(
                "The provided entity type is not a valid queryable type."
            );
        }
        var role = await query.Where(r => EF.Property<object>(r, "Id").Equals(roleId)).FirstOrDefaultAsync();
        if (role is null)
        {
            throw new InvalidOperationException(
                "The provided role id is not valid."
            );
        }
        return role;
    }

    public async Task CreateRoleAsync(TREntity entity)
    {
        try
        {
            var query = dbContext.Set<TREntity>().AsQueryable();
            if (query is null)
            {
                throw new InvalidOperationException(
                    "The provided entity type is not a valid queryable type."
                );
            }

            await dbContext.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to process this operation. Message: {e.Message}");
        }
     
    }

    public async Task UpdateRoleAsync(string roleId, TREntity entity)
    {
        var query = dbContext.Set<TREntity>().AsQueryable();
        if (query is null)
        {
            throw new InvalidOperationException(
                "The provided entity type is not a valid queryable type."
            );
        }
        var role = await query.Where(r => EF.Property<object>(r, "Id").Equals(roleId)).FirstOrDefaultAsync();
        if (role is null)
        {
            throw new InvalidOperationException(
                "The provided role id is not valid."
            );
        }
        dbContext.Update(entity); 
        await dbContext.SaveChangesAsync();
       
    }
    

    public async Task DeleteRoleAsync(string id)
    {
        var query = dbContext.Set<TREntity>().AsQueryable();
        if (query is null)
        {
            throw new InvalidOperationException(
                "The provided entity type is not a valid queryable type."
            );
        }
        var role = await query.Where(r => EF.Property<object>(r, "Id").Equals(id)).FirstOrDefaultAsync();
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
