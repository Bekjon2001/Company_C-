using Company.Data.AvtoGenerate.Entity;
using Company.Dtos.FilterDto;

namespace Company.Dtos
{
    public static class EmployeeProjectSortFilter
    {
        public static IQueryable<EmployeeProject> SortFiter(
            this IQueryable<EmployeeProject> query,
            EmployeeProjectFilterDto filter)
        {
            if(filter.EmployeeProjectId.HasValue && filter.EmployeeProjectId.Value >0)
            {
                query = query.Where(d => d.EmployeeProjectId == filter.EmployeeProjectId);
            }

            if(!string.IsNullOrWhiteSpace(filter.ProjectName))
            {
                query = query.Where(d => d.Project != null && d.Project.ProjectName.Contains(filter.ProjectName));
            }

            if (!string.IsNullOrWhiteSpace(filter.EmployeeFullName))
            {
                query = query.Where(d => d.Employee != null &&
                (d.Employee.FirstName + " " + d.Employee.LastName).Contains(filter.EmployeeFullName));
            }

            //Sort qilish (asosiy ustun: EmployeeProjectId)
            if (filter.OrderType.ToLower() == "desc")
            {
                query = query.OrderByDescending(d => d.EmployeeProjectId);
            }
            else
            {
                query = query.OrderBy(d => d.EmployeeProjectId);
            }

            int page = filter.Page > 0 ? filter.Page : 1;
            int pageSize = filter.PageSize > 0 ? filter.PageSize : 10;

            int skip =(page - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);

            return query;
        }
    }
}
