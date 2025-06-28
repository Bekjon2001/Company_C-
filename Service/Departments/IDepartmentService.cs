using Company.Dtos.FilterDto;
using Company.Repository.Departments.Models;

namespace Company.Service.Departments;

public interface IDepartmentService
{
    int Create(DepartmentCreateDto dto);

    DepartmentDto Update(DepartmentUpdate dto, int id);

    bool Delete(int id);

    DepartmentDto GetById(int id);

    List<DepartmentDto> GetAll(DepartmentFilterDto filter);
}
