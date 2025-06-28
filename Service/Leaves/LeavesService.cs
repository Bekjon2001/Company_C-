using Company.Dtos.FilterDto;
using Company.Repository.Leaves;
using Company.Repository.Leaves.Models;

namespace Company.Service.Leaves
{
    public class LeavesService : ILeavesService
    {
        private readonly ILeaveRepositrory _repository;

        public LeavesService(ILeaveRepositrory repository)
        {
            _repository = repository;
        }

        public int Create(LeaveCreateDto dto)
        {
            if (dto.EmployeeId <= 0)
                throw new ArgumentException("Employee ID noto‘g‘ri");

            if (string.IsNullOrWhiteSpace(dto.Reason))
                throw new ArgumentException("Sabab bo‘sh bo‘lishi mumkin emas");

            if (dto.StartDate > dto.EndDate)
                throw new ArgumentException("Start sana tugash sanasidan katta bo‘lmasligi kerak");

            return _repository.Create(dto);
        }

        public bool Update(LeaveUpdateDto dto)
        {
            var existing = _repository.GetById(dto.LeaveId);
            if (existing == null)
                throw new KeyNotFoundException($"Leave ID {dto.LeaveId} topilmadi");

            if (dto.EmployeeId <= 0)
                throw new ArgumentException("Employee ID noto‘g‘ri");

            if (string.IsNullOrWhiteSpace(dto.Reason))
                throw new ArgumentException("Sabab bo‘sh bo‘lishi mumkin emas");

            if (dto.StartDate > dto.EndDate)
                throw new ArgumentException("Start sana tugash sanasidan katta bo‘lmasligi kerak");

            return _repository.Update(dto);
        }

        public bool Delete(int id)
        {
            var existing = _repository.GetById(id);
            if (existing == null)
                throw new KeyNotFoundException($"Leave ID {id} topilmadi");

            return _repository.Delete(id);
        }

        public LeaveDto GetById(int id)
        {
            var result = _repository.GetById(id);
            if (result == null)
                throw new KeyNotFoundException($"Leave ID {id} topilmadi");

            return result;
        }

        public List<LeaveDto> GetAll(LeaveFilterDto filter)
        {
            return _repository.GetAll(filter);
        }
    }
}
    
