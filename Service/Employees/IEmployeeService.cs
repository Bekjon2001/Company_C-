using Company.Data.AvtoGenerate.Entity;
using Company.Dtos.FilterDto;
using Company.Repository.Employee.Models;

namespace Company.Service.Employees
{
    public interface IEmployeeService
    {
        int Create(EmployeeCreateDto dto);
        bool Update(EmployeeUpdateDto dto);
        bool Delete(int id);
        EmployeeDto GetById(int id);
        List<EmployeeDto> GetAll(EmployeeFilterDto filter);
    }
}
