using AspNetCoreOpeniddictPlus.Core.Dtos;
using AspNetCoreOpeniddictPlus.Core.Extensions;
using AspNetCoreOpeniddictPlus.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreOpeniddictPlus.Core.Services;

public class PermissionService<PEntity, TDbContext>(TDbContext dbContext) : IPermissionService<PEntity> where PEntity : class where TDbContext : DbContext
{
    public async Task<PagedResult<PEntity>> GetRolesAsync(int page = 1, int pageSize = 10, string? orderBy = null)
    {
        var query = dbContext.Set<PEntity>().AsQueryable();
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

    public async Task<PEntity?> GetPermissionByNameAsync(string permissionName)
    {
        var query = dbContext.Set<PEntity>().AsQueryable();
        if (query is null)
        {
            throw new InvalidOperationException(
                "The provided entity type is not a valid queryable type."
            );
        }
        var permission = await query.Where(r => EF.Property<object>(r, "Name").Equals(permissionName)).FirstOrDefaultAsync();
        return permission;
    }

    public async Task<PEntity> GetPermissionByIdAsync(string permissionId)
    {
        var query = dbContext.Set<PEntity>().AsQueryable();
        if (query is null)
        {
            throw new InvalidOperationException(
                "The provided entity type is not a valid queryable type."
            );
        }
        var permission = await query.Where(r => EF.Property<object>(r, "Id").Equals(permissionId)).FirstOrDefaultAsync();
        if (permission is null)
        {
            throw new InvalidOperationException(
                "The provided permission id is not valid."
            );
        }
        return permission;
    }

    public async Task CreatePermissionAsync(PEntity entity)
    {
        try
        {
            var query = dbContext.Set<PEntity>().AsQueryable();
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

    public async Task UpdatePermissionAsync(string permissionId, PEntity entity)
    {
        var query = dbContext.Set<PEntity>().AsQueryable();
        if (query is null)
        {
            throw new InvalidOperationException(
                "The provided entity type is not a valid queryable type."
            );
        }
        var permission = await query.Where(r => EF.Property<object>(r, "Id").Equals(permissionId)).FirstOrDefaultAsync();
        if (permission is null)
        {
            throw new InvalidOperationException(
                "The provided permission id is not valid."
            );
        }
        dbContext.Update(entity); 
        await dbContext.SaveChangesAsync();
    }

    public async Task DeletePermissionAsync(string permissionId)
    {
        var query = dbContext.Set<PEntity>().AsQueryable();
        if (query is null)
        {
            throw new InvalidOperationException(
                "The provided entity type is not a valid queryable type."
            );
        }
        var permission = await query.Where(r => EF.Property<object>(r, "Id").Equals(permissionId)).FirstOrDefaultAsync();
        if (permission is null)
        {
            throw new InvalidOperationException(
                "The provided permission id is not valid."
            );
        }
        dbContext.Remove(permission);
        await dbContext.SaveChangesAsync();
    }
}