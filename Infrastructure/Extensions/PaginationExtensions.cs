using Core.DTOs.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Extensions;

public static class PaginationExtensions
{
    // Extension method to paginate an IQueryable<T> objects and can be used across the application
    // for any entity type
    public static async Task<PaginatedResponseDto<T>> ToPaginatedAsync<T>(this IQueryable<T> query, int pageNumber = 1, int pageSize = 10)
    {
        var totalCount = await query.CountAsync();
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedResponseDto<T>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}