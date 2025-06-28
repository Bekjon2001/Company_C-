using Company.Data.AvtoGenerate.Entity;
using Company.Dtos.FilterDto;

namespace Company.Dtos
{
    public static class SalarieSortFilter
    {
        public static IQueryable<Salarie>SortFilter(
            this IQueryable<Salarie> query,
            SalarieFilterDto filter)
        {
            if (filter.SalaryId.HasValue && filter.SalaryId.Value > 0)
            {
                query = query.Where(d => d.SalaryId == filter.SalaryId);
            }

            if (filter.MinAmount.HasValue)
            {
                query = query.Where(d => d.Amount >= filter.MinAmount);
            }
            if (filter.MaxAmount.HasValue)
            {
                query = query.Where(query => query.Amount <= filter.MaxAmount);
            }

            if (filter.OrderType.ToLower() == "desc")
            {
                query = query.OrderByDescending(d => d.SalaryId);
            }
            else
            {
                query = query.OrderBy(d => d.SalaryId);
            }

            int page = filter.Page > 0 ? filter.Page : 1;
            int pageSize = filter.PageSize > 0 ? filter.PageSize : 10;

            int skip = (page - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);
            return query;
        }
    }
}
