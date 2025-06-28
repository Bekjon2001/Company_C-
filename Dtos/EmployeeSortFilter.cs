using Company.Data.AvtoGenerate.Entity;
using Company.Dtos.FilterDto;

namespace Company.Dtos
{
    public static class EmployeeSortFilter
    {
        public static IQueryable<Employee> SortFilter(this IQueryable<Employee> query, EmployeeFilterDto filter)
        {
            if(filter.EmployeeId.HasValue && filter.EmployeeId.Value > 0)
            {
                query = query.Where(e => e.EmployeeId == filter.EmployeeId);
            }

            if (!string.IsNullOrWhiteSpace(filter.FirstName))
            {
                query = query.Where(e =>e.FirstName == filter.FirstName);
            }

            if (!string.IsNullOrWhiteSpace(filter.LastName))
            {
                query = query.Where(e => e.LastName == filter.LastName);
            }

            if(filter.OrderType?.ToLower() == "desc")
            {
                query = query.OrderByDescending(e => e.EmployeeId);
            }
            else
            {
                query = query.OrderBy(e => e.EmployeeId);
            }

            if (filter.PageSize <= 0)
                filter.PageSize = 10;
            if (filter.Page <= 0)
                filter.Page = 1;

            int skip = (filter.Page - 1) * filter.PageSize;
            query = query.Skip(skip).Take(filter.PageSize);
            return query;
        }
    }
}
