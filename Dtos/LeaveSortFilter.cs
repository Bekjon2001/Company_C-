using Company.Data.AvtoGenerate.Entity;
using Company.Dtos.FilterDto;

namespace Company.Dtos
{
    public static class LeaveSortFilter
    {
        public static IQueryable<Leave>SortFliter(
            this IQueryable<Leave> query,
            LeaveFilterDto filter)
        {
            if (filter.LeaveID.HasValue &&  filter.LeaveID.Value > 0)
            {
                query = query.Where( d => d.LeaveId == filter.LeaveID.Value );
            }

            
            if (filter.OrderType.ToLower() == "desc")
            {
                query = query.OrderByDescending(d => d.LeaveId);
            }
            else
            {
                query = query.OrderBy(d => d.LeaveId);
            }

            int page = filter.Page > 0 ? filter.Page : 1;
            int pageSize = filter.PageSize > 0 ? filter.PageSize : 10;

            int skip = (page - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);
            return query;
        }
    }
}
