using Company.Dtos.FilterDto;
using Company.Repository.Departments.Models;
using Company.Repository.Positions.Models;

namespace Company.Repository.Positions;

public interface IPositionRepository
{
    int Create(PositionCreateDto dto);
    bool Update(PositionUpdate dto,int id);
    bool Delete(int id);
    PositionDto GetById(int id);
    List<PositionDto> GetAll(PositionFilterDto filter);
    Task<byte[]> Print();
}
