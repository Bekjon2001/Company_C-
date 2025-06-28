using Company.Dtos.FilterDto;
using Company.Repository.Departments.Models;

namespace Company.Repository.Departments;

public interface IDepartmentRepository
{
    int Create(DepartmentCreateDto dto);

    DepartmentDto Update(DepartmentUpdate dto, int id);

    bool Delete(int  id);

    DepartmentDto GetById(int id);

    List<DepartmentDto> GetAll( DepartmentFilterDto filter);
}
