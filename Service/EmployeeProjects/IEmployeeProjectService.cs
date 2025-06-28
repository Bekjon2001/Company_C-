using Company.Dtos.FilterDto;
using Company.Repository.EmployeeProjects.Models;

namespace Company.Service.EmployeeProjects
{
    public interface IEmployeeProjectService
    {
        int Create(EmployeeProjectCreateDto dto);
        bool Update(EmployeeProjectUpdateDto dto);
        bool Delete(int id);
        EmployeeProjectDto GetById(int id);
        List<EmployeeProjectDto> GetAll( EmployeeProjectFilterDto filter );
    }
}
