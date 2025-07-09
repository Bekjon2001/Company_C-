using Company.Data.AvtoGenerate.Entity;
using Company.Dtos;
using Company.Dtos.FilterDto;
using Company.Repository.Employee;
using Company.Repository.Employee.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

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

    public async Task<int> ImportFromExcelAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return 0;

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        using var package = new ExcelPackage(stream);
        var worksheet = package.Workbook.Worksheets[0];
        int rowCount = worksheet.Dimension.Rows;
        //int companyId = int.Parse(worksheet.Cells[row, 8].Text);
        var employees = new List<Employee>();

        for (int row = 2; row <= rowCount; row++)
        {
            var employee = new Employee
            {
                FirstName = worksheet.Cells[row, 1].Text,
                LastName = worksheet.Cells[row, 2].Text,
                Email = worksheet.Cells[row, 3].Text,
                Phone = worksheet.Cells[row, 4].Text,
                Hiredate = DateTime.TryParse(worksheet.Cells[row, 5].Text, out var hd) ? hd.ToUniversalTime() : null,
                DateOfBirth = DateTime.TryParse(worksheet.Cells[row, 6].Text, out var dob) ? dob.ToUniversalTime() : null,
                Address = worksheet.Cells[row, 7].Text,
                CompanyId = int.Parse(worksheet.Cells[row, 8].Text),
                DepartmentId = int.Parse(worksheet.Cells[row, 9].Text),
                PositionId = int.Parse(worksheet.Cells[row, 10].Text),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            employees.Add(employee);
        }

        _context.Employee.AddRange(employees);
        return await _context.SaveChangesAsync();
    }

    public async Task<byte[]> ExportToExcelAsync()
    {
        var employees = await _context.Employee
                .Include(e => e.Company)
                .Include(e => e.Department)
                .Include(e => e.Position)
                .ToListAsync();

        var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Employees");

        var headers = new[]
        {
        "First Name", "Last Name", "Email", "Phone",
        "Hire Date", "Date of Birth", "Address",
        "Company Name", "Department Name", "Position Name"
    };

        for (int i = 0; i < headers.Length; i++)
            worksheet.Cells[1, i + 1].Value = headers[i];

        using (var range = worksheet.Cells[1, 1, 1, headers.Length])
        {
            range.Style.Font.Name = "Arial Black";
            range.Style.Font.Size = 11;
            range.Style.Font.Bold = true;
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            range.Style.WrapText = false;
        }

        for (int i = 0; i < employees.Count; i++)
        {
            var e = employees[i];
            worksheet.Cells[i + 2, 1].Value = e.FirstName;
            worksheet.Cells[i + 2, 2].Value = e.LastName;
            worksheet.Cells[i + 2, 3].Value = e.Email;
            worksheet.Cells[i + 2, 4].Value = e.Phone;
            worksheet.Cells[i + 2, 5].Value = e.Hiredate?.ToString("yyyy-MM-dd");
            worksheet.Cells[i + 2, 6].Value = e.DateOfBirth?.ToString("yyyy-MM-dd");
            worksheet.Cells[i + 2, 7].Value = e.Address;

            worksheet.Cells[i + 2, 8].Value = e.Company?.Name?? "Noma'lum";
            worksheet.Cells[i + 2, 9].Value = e.Department?.Name?? "Noma'lum";
            worksheet.Cells[i + 2, 10].Value = e.Position?.PositionName?? "Noma'lum";
        }
        //Afataman ustunlarni enga moslashtradi 
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        return package.GetAsByteArray();
    }


}
