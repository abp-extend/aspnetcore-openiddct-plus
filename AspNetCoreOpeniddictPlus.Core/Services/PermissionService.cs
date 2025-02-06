
using AspNetCoreOpeniddictPlus.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreOpeniddictPlus.Core.Services;

public class PermissionService<PEntity, TDbContext>(TDbContext dbContext)
    : IPermissionService<PEntity> where PEntity : class where TDbContext : DbContext
{
    public async Task<IQueryable<PEntity>> GetPermissions()
    {
        return dbContext.Set<PEntity>().AsQueryable();
    }

    public async Task<PEntity?> GetPermissionByNameAsync(string permissionName)
    {
        var query = dbContext.Set<PEntity>().AsQueryable();
        return await query.FirstOrDefaultAsync(p => EF.Property<object>(p, "Name").Equals(permissionName));
    }

    public async Task<PEntity> GetPermissionByIdAsync(string permissionId)
    {
        var query = dbContext.Set<PEntity>().AsQueryable();

        var permission = await query.FirstOrDefaultAsync(p => EF.Property<object>(p, "Id").Equals(permissionId));
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
        var alreadyExist = await GetPermissionByNameAsync(EF.Property<string>(entity, "Name"));
        if (alreadyExist is not null)
        {
            throw new InvalidOperationException(
                "The provided permission name already exists."
            );
        }

        await dbContext.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdatePermissionAsync(string permissionId, PEntity entity)
    {
        var query = dbContext.Set<PEntity>().AsQueryable();
      

        var permission =
            await query.Where(r => EF.Property<object>(r, "Id").Equals(permissionId)).FirstOrDefaultAsync();
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
        var permission = await GetPermissionByIdAsync(permissionId);
        dbContext.Remove(permission);
        await dbContext.SaveChangesAsync();
    }
}