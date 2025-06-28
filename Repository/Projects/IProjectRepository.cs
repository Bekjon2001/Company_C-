using Company.Dtos.FilterDto;
using Company.Repository.Projects.Models;

namespace Company.Repository.Projects;

public interface IProjectRepository
{
    int Create(ProjectCreateDto dto);
    bool Update(ProjectUpdateDto dto, int id);
    bool Delete(int id);
    ProjectDto GetById(int id);
    List<ProjectDto> GetAll(ProjectFilterDto filter);
}
