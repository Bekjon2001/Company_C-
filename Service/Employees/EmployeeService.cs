using Company.Data.AvtoGenerate.Entity;
using Company.Dtos.FilterDto;
using Company.Repository.Employee;
using Company.Repository.Employee.Models;

namespace Company.Service.Employees
{
    public class EmployeeService: IEmployeeService
    {
        private readonly IEmployeeRepositoriy _repository;

        public EmployeeService(IEmployeeRepositoriy repository)
        {
            _repository = repository;
        }

        public int Create(EmployeeCreateDto dto)
        {
            // Majburiy maydonlarni tekshirish
            if (string.IsNullOrWhiteSpace(dto.FirstName))
                throw new ArgumentException("Ism bo‘sh bo‘lmasligi kerak");

            if (string.IsNullOrWhiteSpace(dto.LastName))
                throw new ArgumentException("Familiya bo‘sh bo‘lmasligi kerak");

            if (dto.Hiredate  > DateTime.Today)
                throw new ArgumentException("Ishga qabul sanasi kelajakda bo‘lishi mumkin emas");

            if (dto.DateOfBirth > DateTime.Today)
                throw new ArgumentException("Tug‘ilgan sana kelajakda bo‘lishi mumkin emas");

            if (dto.CompanyId <= 0 || dto.DepartmentId <= 0 || dto.PositionId <= 0)
                throw new ArgumentException("Company, Department yoki Position ID noto‘g‘ri");

            return _repository.Create(dto);
        }

        public bool Update(EmployeeUpdateDto dto)
        {
            var existing = _repository.GetById(dto.EmployeeId);
            if (existing == null)
                throw new KeyNotFoundException($"Employee ID {dto.EmployeeId} topilmadi");

            if (dto.Hiredate > DateTime.Today)
                throw new ArgumentException("Ishga qabul sanasi kelajakda bo‘lishi mumkin emas");

            if (dto.DateOfBirth > DateTime.Today)
                throw new ArgumentException("Tug‘ilgan sana kelajakda bo‘lishi mumkin emas");

            return _repository.Update(dto);
        }

        public bool Delete(int id)
        {
            var existing = _repository.GetById(id);
            if (existing == null)
                throw new KeyNotFoundException($"Employee ID {id} mavjud emas");

            return _repository.Delete(id);
        }

        public EmployeeDto GetById(int id)
        {
            var result = _repository.GetById(id);
            if (result == null)
                throw new KeyNotFoundException($"Employee ID {id} topilmadi");

            return result;
        }

        public List<EmployeeDto> GetAll( EmployeeFilterDto filter)
        {
            return _repository.GetAll(filter);
        }
    }
}
