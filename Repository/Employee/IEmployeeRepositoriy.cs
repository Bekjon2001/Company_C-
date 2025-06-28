using Company.Dtos.FilterDto;
using Company.Repository.Employee.Models;

namespace Company.Repository.Employee;

public interface IEmployeeRepositoriy
{
    int Create(EmployeeCreateDto dto);
    bool Update(EmployeeUpdateDto dto);
    bool Delete(int id);
    EmployeeDto GetById(int id);
    List<EmployeeDto> GetAll(EmployeeFilterDto filter);

}
