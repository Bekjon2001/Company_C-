using Company.Dtos.FilterDto;
using Company.Repository.Salaries;
using Company.Repository.Salaries.Models;

namespace Company.Service.Salaries
{
    public class SalarieService : ISalarieService
    {
        private readonly ISalarieRepositoriy _repository;

        public SalarieService(ISalarieRepositoriy repository)
        {
            _repository = repository;
        }

        public int Create(SalarieCreateDto dto)
        {
            if (dto.EmployeeId <= 0)
                throw new ArgumentException("Employee ID noto‘g‘ri");

            if (dto.Amount <= 0)
                throw new ArgumentException("Maosh miqdori 0 dan katta bo‘lishi kerak");

            return _repository.Create(dto);
        }

        public bool Update(SalarieUpdateDto dto)
        {
            var existing = _repository.GetById(dto.SalaryId);
            if (existing == null)
                throw new KeyNotFoundException($"Salarie ID {dto.SalaryId} topilmadi");

            if (dto.EmployeeId <= 0)
                throw new ArgumentException("Employee ID noto‘g‘ri");

            if (dto.Amount <= 0)
                throw new ArgumentException("Maosh miqdori 0 dan katta bo‘lishi kerak");

            return _repository.Update(dto);
        }

        public bool Delete(int id)
        {
            var existing = _repository.GetById(id);
            if (existing == null)
                throw new KeyNotFoundException($"Salarie ID {id} mavjud emas");

            return _repository.Delete(id);
        }

        public SalarieDto GetById(int id)
        {
            var result = _repository.GetById(id);
            if (result == null)
                throw new KeyNotFoundException($"Salarie ID {id} topilmadi");

            return result;
        }

        public List<SalarieDto> GetAll(SalarieFilterDto filter)
        {
            return _repository.GetAll(filter);
        }
    }
}

