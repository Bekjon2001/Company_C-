using Company.Dtos.FilterDto;
using Company.Repository.EmployeeProjects;
using Company.Repository.EmployeeProjects.Models;

namespace Company.Service.EmployeeProjects
{
    public class EmployeeProjectService: IEmployeeProjectService
    {
        private readonly IEmployeeProjectRepository _repository;

        public EmployeeProjectService(IEmployeeProjectRepository repository)
        {
            _repository = repository;
        }

        public int Create(EmployeeProjectCreateDto dto)
        {
            if (dto.EmployeeId <= 0)
                throw new ArgumentException("Employee ID noto‘g‘ri");

            if (dto.ProjectId <= 0)
                throw new ArgumentException("Project ID noto‘g‘ri");

            return _repository.Create(dto);
        }

        public bool Update(EmployeeProjectUpdateDto dto)
        {
            var existing = _repository.GetById(dto.EmployeeProjectId);
            if (existing == null)
                throw new KeyNotFoundException($"EmployeeProject ID {dto.EmployeeProjectId} topilmadi");

            if (dto.EmployeeId <= 0)
                throw new ArgumentException("Employee ID noto‘g‘ri");

            if (dto.ProjectId <= 0)
                throw new ArgumentException("Project ID noto‘g‘ri");

            return _repository.Update(dto);
        }

        public bool Delete(int id)
        {
            var existing = _repository.GetById(id);
            if (existing == null)
                throw new KeyNotFoundException($"EmployeeProject ID {id} topilmadi");

            return _repository.Delete(id);
        }

        public EmployeeProjectDto GetById(int id)
        {
            var result = _repository.GetById(id);
            if (result == null)
                throw new KeyNotFoundException($"EmployeeProject ID {id} mavjud emas");

            return result;
        }

        public List<EmployeeProjectDto> GetAll( EmployeeProjectFilterDto filter )
        {
            return _repository.GetAll(filter);
        }

    }
}
