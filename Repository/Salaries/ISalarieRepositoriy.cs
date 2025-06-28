using Company.Dtos.FilterDto;
using Company.Repository.Salaries.Models;

namespace Company.Repository.Salaries;

public interface ISalarieRepositoriy
{
    int Create(SalarieCreateDto dto);
    bool Update(SalarieUpdateDto dto);
    bool Delete(int id);
    SalarieDto GetById(int id);
    List<SalarieDto> GetAll( SalarieFilterDto filter );
}
