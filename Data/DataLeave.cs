using Company.Data.AvtoGenerate.Entity;
using Company.Data.dataContext;
using Company.Dtos;
using Company.Dtos.FilterDto;
using Company.Repository.Leaves;
using Company.Repository.Leaves.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.Data;

public class DataLeave : ILeaveRepositrory
{
    private readonly DbContextdta _context;

    public DataLeave(DbContextdta context)
    { 
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public int Create(LeaveCreateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var leave = new Leave
        {
            EmployeeId = dto.EmployeeId,
            Reason = dto.Reason,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Leaves.Add(leave);
        _context.SaveChanges();
        return leave.LeaveId;
    }

    public bool Update(LeaveUpdateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var leave = _context.Leaves.Find(dto.LeaveId);
        if (leave == null) return false;

        leave.Reason = dto.Reason ?? leave.Reason;
        leave.StartDate = dto.StartDate ?? leave.StartDate;
        leave.EndDate = dto.EndDate ?? leave.EndDate;
        leave.UpdatedAt = DateTime.UtcNow;

        _context.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var leave = _context.Leaves.Find(id);
        if (leave == null) return false;

        _context.Leaves.Remove(leave);
        _context.SaveChanges();
        return true;
    }

    public LeaveDto GetById(int id)
    {
        var leave = _context.Leaves
            .Include(l => l.Employee)
            .FirstOrDefault(l => l.LeaveId == id);

        if (leave == null) return null;

        return new LeaveDto
        {
            LeaveID = leave.LeaveId,
            EmployeeID = leave.EmployeeId,
            Reason = leave.Reason,
            StartDate = leave.StartDate,
            EndDate = leave.EndDate,
            CreatedAt = leave.CreatedAt,
            UpdatedAt = leave.UpdatedAt,
            EmployeeName = leave.Employee != null
                ? $"{leave.Employee.FirstName} {leave.Employee.LastName}"
                : null
        };
    }

    public List<LeaveDto> GetAll( LeaveFilterDto filter )
    {
        var query = _context.Leaves
            .Include(l => l.Employee)
            .SortFliter(filter)
            .AsQueryable();
        return query
            .Select(leave => new LeaveDto
            {
                LeaveID = leave.LeaveId,
                EmployeeID = leave.EmployeeId,
                Reason = leave.Reason,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                CreatedAt = leave.CreatedAt,
                UpdatedAt = leave.UpdatedAt,
                EmployeeName = leave.Employee != null
                    ? $"{leave.Employee.FirstName} {leave.Employee.LastName}"
                    : null
            })
            .ToList();
    }

}


