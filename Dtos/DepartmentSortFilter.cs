using Company.Data.AvtoGenerate.Entity;
using Company.Dtos.FilterDto;

namespace Company.Dtos;

public static class DepartmentSortFilter
{
    public static IQueryable<Department> SortFilter(this IQueryable<Department> query,DepartmentFilterDto filter)
    {
        // Filter: DepartmentId bo‘yicha
        if (filter.DepartmentId.HasValue && filter.DepartmentId.Value > 0)
        {
            query = query.Where(d => d.DepartmentId == filter.DepartmentId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(d => d.Name.Contains(filter.Name));
        }

        // Sort: OrderType (asc yoki desc)
        if (filter.OrderType?.ToLower() == "desc")
        {
            query = query.OrderByDescending(d => d.DepartmentId);
        }
        else
        {
            query = query.OrderBy(d => d.DepartmentId);
        }

        // Paging
        if (filter.PageSize <= 0)
            filter.PageSize = 5;

        if (filter.Page <= 0)
            filter.Page = 1;

        int skip = (filter.Page - 1) * filter.PageSize;
        query = query.Skip(skip).Take(filter.PageSize);

        // ✅ Har doim query return qilinadi
        return query;
    }
}
