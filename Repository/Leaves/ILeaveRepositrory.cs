using Company.Dtos.FilterDto;
using Company.Repository.Leaves.Models;

namespace Company.Repository.Leaves;

public interface ILeaveRepositrory
{
    int Create(LeaveCreateDto dto);
    bool Update(LeaveUpdateDto dto);
    bool Delete(int id);
    LeaveDto GetById(int id);
    List<LeaveDto> GetAll( LeaveFilterDto filter );
    Task<byte[]> Print();
}
