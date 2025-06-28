using Company.Dtos.FilterDto;
using Company.Repository.Positions;
using Company.Repository.Positions.Models;

namespace Company.Service.Positions
{
    public class PositionsService : IPositionsService
    {
        private readonly IPositionRepository _repository;

        public PositionsService(IPositionRepository repository)
        {
            _repository = repository;
        }

        public int Create(PositionCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PositionName))
                throw new ArgumentException("Lavozim nomi bo‘sh bo‘lishi mumkin emas");

            return _repository.Create(dto);
        }

        public bool Update(PositionUpdate dto, int id)
        {
            var existing = _repository.GetById(id);
            if (existing == null)
                throw new KeyNotFoundException($"Position ID {id} topilmadi");

            if (string.IsNullOrWhiteSpace(dto.PositionName))
                throw new ArgumentException("Lavozim nomi bo‘sh bo‘lishi mumkin emas");

            return _repository.Update(dto, id);
        }

        public bool Delete(int id)
        {
            var existing = _repository.GetById(id);
            if (existing == null)
                throw new KeyNotFoundException($"Position ID {id} mavjud emas");

            return _repository.Delete(id);
        }

        public PositionDto GetById(int id)
        {
            var result = _repository.GetById(id);
            if (result == null)
                throw new KeyNotFoundException($"Position ID {id} topilmadi");

            return result;
        }

        public List<PositionDto> GetAll(PositionFilterDto filter)
        {
            return _repository.GetAll(filter);
        }
    }
}

