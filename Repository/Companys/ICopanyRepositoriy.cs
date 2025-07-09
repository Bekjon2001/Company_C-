using Company.Dtos.FilterDto;
using Company.Repository.Company.Models;


namespace Company.Repository.Company;

public interface ICopanyRepositoriy 
{
    int Create(CompanyCreateDto dto);

    bool Update(CompanyUpdateDto dto,int id);

    bool Delete(int id);

    CompanyDto GetById(int id);
    Task<List<CompanyDto>> GetAllAsync(CompanyFilterDto filter);

    Task<byte[]> Print();
}
