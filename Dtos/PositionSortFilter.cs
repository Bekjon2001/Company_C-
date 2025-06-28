using Company.Data.AvtoGenerate.Entity;
using Company.Dtos.FilterDto;

namespace Company.Dtos
{
    public static class PositionSortFilter
    {
        public static IQueryable<Position>SortFilter(
            this IQueryable<Position> query,
            PositionFilterDto filter)
        {
            if (filter.PositionId.HasValue &&  filter.PositionId.Value > 0)
            {
                query = query.Where(d => d.PositionId == filter.PositionId);
            }

            if (!string.IsNullOrWhiteSpace(filter.PositionName))
            {
                query = query.Where(x => x.PositionName.Contains(filter.PositionName));
            }

            if (filter.OrderType.ToLower() == "desc")
            {
                query = query.OrderByDescending(d => d.PositionId);
            }
            else
            {
                query = query.OrderBy(d => d.PositionId);
            }

            int page = filter.Page > 0 ? filter.Page : 1;
            int pageSize = filter.PageSize > 0 ? filter.PageSize : 10;

            int skip = (page - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);
            return query;


        }
    }
}
