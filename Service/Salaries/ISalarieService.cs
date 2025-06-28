using Company.Dtos.FilterDto;
using Company.Repository.Salaries.Models;

namespace Company.Service.Salaries
{
    public interface ISalarieService
    {
        int Create(SalarieCreateDto dto);
        bool Update(SalarieUpdateDto dto);
        bool Delete(int id);
        SalarieDto GetById(int id);
        List<SalarieDto> GetAll( SalarieFilterDto filter );
    }
}
