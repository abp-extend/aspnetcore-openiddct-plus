using AspNetCoreOpeniddictPlus.Core.Dtos;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreOpeniddictPlus.Core.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        int page,
        int pageSize
    )
    {
        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<T>
        {
            Items = items,
            TotalCount = totalCount,
            PageSize = pageSize,
            CurrentPage = page,
        };
    }
}
