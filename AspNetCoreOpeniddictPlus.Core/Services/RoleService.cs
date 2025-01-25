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

    public async Task<TREntity> GetRoleByIdAsync(Guid roleId)
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

    public async Task<Guid> CreateRoleAsync(CreateRoleDto createRoleDto)
    {
        var query = dbContext.Set<TREntity>().AsQueryable();
        if (query is null)
        {
            throw new InvalidOperationException(
                "The provided entity type is not a valid queryable type."
            );
        }
        return Guid.NewGuid();
      
    }

    public async Task<Guid> UpdateRoleAsync(Guid roleId, UpdateRoleDto updateRoleDto)
    {
        var query = dbContext.Set<TREntity>().AsQueryable();
        if (query is null)
        {
            throw new InvalidOperationException(
                "The provided entity type is not a valid queryable type."
            );
        }
        var role = await query.Where(r => EF.Property<object>(r, "Id").Equals(roleId)).FirstOrDefaultAsync();
        return Guid.NewGuid();
    }
    

    public Task<Guid> DeleteRoleAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
