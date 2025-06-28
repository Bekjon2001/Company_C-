using Company.Dtos.FilterDto;
using Company.Repository.Positions.Models;

namespace Company.Service.Positions
{
    public interface IPositionsService
    {
        int Create(PositionCreateDto dto);
        bool Update(PositionUpdate dto, int id);
        bool Delete(int id);
        PositionDto GetById(int id);
        List<PositionDto> GetAll(PositionFilterDto filter);
    }
}
