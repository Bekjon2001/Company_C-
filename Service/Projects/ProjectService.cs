using Company.Dtos.FilterDto;
using Company.Repository.Projects;
using Company.Repository.Projects.Models;

namespace Company.Service.Projects
{
    public class ProjectService : IProjectService
    {

        private readonly IProjectRepository _repository;
        public ProjectService(IProjectRepository repository)
        {
            _repository = repository;
        }

        public int Create(ProjectCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ProjectName))
                throw new ArgumentException("Loyiha nomi bo‘sh bo‘lmasligi kerak");

            if (dto.StartDate > dto.EndDate)
                throw new ArgumentException("Boshlanish sanasi tugash sanasidan oldin bo‘lishi kerak");

            if (dto.CompanyId <= 0)
                throw new ArgumentException("Company ID noto‘g‘ri");

            return _repository.Create(dto);
        }

        public bool Update(ProjectUpdateDto dto, int id)
        {
            var existing = _repository.GetById(id);
            if (existing == null)
                throw new KeyNotFoundException($"Project ID {id} topilmadi");

            if (string.IsNullOrWhiteSpace(dto.ProjectName))
                throw new ArgumentException("Loyiha nomi bo‘sh bo‘lmasligi kerak");

            if (dto.StartDate > dto.EndDate)
                throw new ArgumentException("Boshlanish sanasi tugash sanasidan oldin bo‘lishi kerak");

            return _repository.Update(dto, id);
        }

        public bool Delete(int id)
        {
            var existing = _repository.GetById(id);
            if (existing == null)
                throw new KeyNotFoundException($"Project ID {id} mavjud emas");

            return _repository.Delete(id);
        }

        public ProjectDto GetById(int id)
        {
            var result = _repository.GetById(id);
            if (result == null)
                throw new KeyNotFoundException($"Project ID {id} topilmadi");

            return result;
        }

        public List<ProjectDto> GetAll(ProjectFilterDto filter)
        {
            return _repository.GetAll(filter);
        }

    }
}
