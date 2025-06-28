using Company.Data.AvtoGenerate.Entity;
using Company.Dtos;
using Company.Dtos.FilterDto;
using Company.Repository.Employee;
using Company.Repository.Employee.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Company.Data;

public class DataEmployee : IEmployeeRepositoriy
{
    private readonly dataContext.DbContextdta _context;

    public DataEmployee(dataContext.DbContextdta context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public int Create(EmployeeCreateDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof (dto));

        var employee = new Employee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            Hiredate = dto.Hiredate,
            DateOfBirth = dto.DateOfBirth,
            Address = dto.Address,
            CompanyId = dto.CompanyId,
            DepartmentId = dto.DepartmentId,
            PositionId = dto.PositionId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _context.Employee.Add(employee);
        _context.SaveChanges();
        return employee.EmployeeId;
    }
    public bool Update(EmployeeUpdateDto dto)
    {
        if ( dto == null) throw new ArgumentNullException( nameof(dto));

        var employee = _context.Employee.First();
        if (employee == null) return false;
        employee.FirstName = dto.FirstName ?? employee.FirstName;
        employee.LastName = dto.LastName ?? employee.LastName;
        employee.Email = dto.Email ?? employee.Email;
        employee.Phone = dto.Phone ?? employee.Phone;
        employee.Hiredate = DateTime.UtcNow;
        employee.DateOfBirth = DateTime.UtcNow;
        employee.Address = dto.Address ?? employee.Address;
        employee.CompanyId = dto.CompanyId;
        employee.DepartmentId = dto.DepartmentId;   
        employee.PositionId = dto.PositionId;
        employee.UpdatedAt = DateTime.UtcNow;

        _context.SaveChanges();
        return true;



    }

    public bool Delete(int id)
    {
        var employee = _context.Employee.Find(id);
        if (employee == null) return false;
        _context.Employee.Remove(employee);

        _context.SaveChanges();
        return true;
    }

    public EmployeeDto GetById(int id)
    {
        var employee = _context.Employee
            .Include(e => e.Company)
            .Include(e => e.Department)
            .Include(e => e.Position)
            .FirstOrDefault(e => e.EmployeeId == id);

        if (employee == null) return null;

        return new EmployeeDto
        {
            EmployeeId = employee.EmployeeId,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Phone = employee.Phone,
            Hiredate = employee.Hiredate,
            DateOfBirth = employee.DateOfBirth,
            Address = employee.Address,
            CompanyId = employee.CompanyId,
            DepartmentId = employee.DepartmentId,
            PositionId = employee.PositionId,
            CreatedAt = employee.CreatedAt,
            UpdatedAt = employee.UpdatedAt,

            // Agar CompanyDto ichida CompanyName bo‘lsa:
            // CompanyName = employee.Company?.Name,
            // DepartmentName = employee.Department?.Name,
            // PositionName = employee.Position?.PositionName
        };
    }

    public List<EmployeeDto> GetAll(EmployeeFilterDto filter )
    {
        var employees = _context.Employee
            .Include(e => e.Company)
            .Include(e => e.Department)
            .Include(e => e.Position)
            .SortFilter(filter) 
            .ToList();

        var result = employees.Select(employee => new EmployeeDto
        {
            EmployeeId = employee.EmployeeId,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Phone = employee.Phone,
            Hiredate = employee.Hiredate,
            DateOfBirth = employee.DateOfBirth,
            Address = employee.Address,
            CompanyId = employee.CompanyId,
            DepartmentId = employee.DepartmentId,
            PositionId = employee.PositionId,
            CreatedAt = employee.CreatedAt,
            UpdatedAt = employee.UpdatedAt,
            CompanyName = employee.Company?.Name,
            DepartmentName = employee.Department?.Name,
            PositionName = employee.Position?.PositionName

        }).ToList();

        return result;
    }

}
