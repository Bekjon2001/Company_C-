using Company.Data.AvtoGenerate.Entity;
using Company.Dtos.FilterDto;
namespace Company.Dtos;

public static class CompanySortFilter
{
    public static IQueryable<Companys> SortFilter(this IQueryable<Companys> query, CompanyFilterDto filter)
    {
        if(filter.CompanyId.HasValue && filter.CompanyId.Value > 0)
            query = query.Where(s => s.CompanyId == filter.CompanyId);
        if(!string.IsNullOrWhiteSpace(filter.CompanyName))
        query = query.Where(x => x.Name.Contains(filter.CompanyName));

        if(filter.OrderType=="desc") 
            query = query.OrderByDescending(s =>s.CompanyId);
        else 
            query = query.OrderBy(s => s.CompanyId);

        if (filter.PageSize <=  0)
            filter.PageSize = 5;

        if (filter.Page <= 0)
            filter.Page = 1;

        if (filter.PageSize > 0 && filter.Page > 0)
        {
            int skip = (filter.Page - 1) * filter.PageSize;
            query = query.Skip(skip).Take(filter.PageSize);
        }
        return query;
    }
}
