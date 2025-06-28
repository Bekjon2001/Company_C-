using Company.Data.AvtoGenerate.Entity;
using Company.Dtos.FilterDto;

namespace Company.Dtos
{
    public static class ProjectSortFilter
    {
        public static IQueryable<Project> SortFilter(
            this IQueryable<Project> query,
            ProjectFilterDto filter)
        {
            if (filter.ProjectId.HasValue &&  filter.ProjectId.Value > 0)
            {
                query = query.Where(d => d.ProjectId == filter.ProjectId);
            }

            if (!string.IsNullOrWhiteSpace(filter.ProjectName))
            {
                query = query.Where(x => x.ProjectName.Contains(filter.ProjectName));
            }

            if (filter.OrderType.ToLower() == "desc")
            {
                query = query.OrderByDescending(d => d.ProjectId);
            }
            else
            {
                query = query.OrderBy(d => d.ProjectId);
            }

            int page = filter.Page > 0 ? filter.Page : 1;
            int pageSize = filter.PageSize > 0 ? filter.PageSize : 10;

            int skip = (page - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);
            return query;
        }
    }
}
