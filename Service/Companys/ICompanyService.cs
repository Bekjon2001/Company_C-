using Company.Dtos.FilterDto;
using Company.Repository.Company.Models;

namespace Company.Service.Companys;

public interface ICompanyService
{
    int Create(CompanyCreateDto dto);

    bool Update(CompanyUpdateDto dto, int id);

    bool Delete(int id);

    CompanyDto GetById(int id);
    Task<List<CompanyDto>> GetAll(CompanyFilterDto filter);
}
