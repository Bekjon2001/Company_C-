using Bogus.DataSets;
using Company.Data.AvtoGenerate.Entity;
using Company.Data.dataContext;
using Company.Dtos;
using Company.Dtos.FilterDto;
using Company.Repository.Company;
using Company.Repository.Company.Models;
using Company.Repository.Departments.Models;
using Company.Repository.Employee.Models;
using Company.Repository.Positions.Models;
using Company.Repository.Projects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace Company.Data;

public class DataCompany : ICopanyRepositoriy 
{
    private readonly dataContext.DbContextdta _context;

    public DataCompany(dataContext.DbContextdta context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public int Create(CompanyCreateDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));



        var company = new AvtoGenerate.Entity.Companys
        {
            Name = dto.Name,
            Location = dto.Location,
            PhoneNumber = dto.PhoneNumber,
            FoundedYear = dto.FoundedYear ?? DateTime.MinValue, // null bo‘lsa, minimal sanani beramiz
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Companies.Add(company);
        _context.SaveChanges();

        return company.CompanyId;

    }

    public bool Update(CompanyUpdateDto dto, int id)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        var company = _context.Companies.Find(id);
        if (company == null) return false;

        company.Name = dto.Name ?? company.Name; // Null bo‘lsa, eski qiymatni saqlash
        company.Location = dto.Location ?? company.Location;
        company.PhoneNumber = dto.PhoneNumber ?? company.PhoneNumber;
        company.FoundedYear = dto.FoundedYear;
        company.UpdatedAt = DateTime.UtcNow;

        _context.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var company = _context.Companies.Find(id);
        if (company == null) return false;

        _context.Companies.Remove(company);
        _context.SaveChanges();
        return true;
    }

    public CompanyDto GetById(int id)
    {
        var company = _context.Companies
            .Include(c => c.Departments)
            .Include(c => c.Employees)
            .Include(c => c.Projects).FirstOrDefault(c => c.CompanyId == id);
        if (company == null) return null;


        return new CompanyDto
        {
            CompanyId = company.CompanyId,
            Name = company.Name,
            Location = company.Location,
            PhoneNumber = company.PhoneNumber,
            FoundedYear = company.FoundedYear,
            CreatedAt = company.CreatedAt,
            UpdatedAt = company.UpdatedAt,

            Departments = company.Departments?.Select(d => new DepartmentDto
            {
                DepartmentId = d.DepartmentId,
                Name = d.Name,
                CompanyId = d.CompanyId,
                CompanyName = d.Company != null ? d.Company.Name : null,
                UpdatedAt = d.UpdatedAt,
                CreatedAt = d.CreatedAt,
            }).ToList(),

            Employees = company.Employees?.Select(d => new EmployeeDto
            {
                EmployeeId = d.EmployeeId,
                LastName = d.LastName,
                FirstName = d.FirstName,
                Email = d.Email,
                Phone = d.Phone,
                Hiredate = d.Hiredate,
                DateOfBirth = d.Hiredate,
                Address = d.Address,
                CompanyId = d.CompanyId,
                DepartmentId = d.DepartmentId,
                PositionId = d.PositionId,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt,
                CompanyName = d.Company != null ? d.Company.Name : null,
                DepartmentName = d.Department != null ? d.Department.Name : null,
                PositionName = d.Position != null ? d.Position.PositionName:null
            }).ToList(),

            Projects = company.Projects?.Select(d => new ProjectDto
            {
                ProjectId = d.ProjectId,
                ProjectName = d.ProjectName,
                Description = d.Description

            }).ToList()
        };
    }

    public async Task<List<CompanyDto>> GetAllAsync(CompanyFilterDto filter)
    {
        return await _context.Companies
            .Include(c => c.Departments)
            .Include(c => c.Employees)
            .Include(c => c.Projects)
            //.AsSplitQuery()
            .AsQueryable()
            .SortFilter(filter)
            .Select(c => new CompanyDto
            {
                CompanyId = c.CompanyId,
                Name = c.Name,
                Location = c.Location,
                PhoneNumber = c.PhoneNumber,
                FoundedYear = c.FoundedYear,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                Departments = c.Departments.Select(d => new DepartmentDto
                {
                    DepartmentId = d.DepartmentId,
                    Name = d.Name
                }).ToList(),
                Employees = c.Employees.Select(d => new EmployeeDto
                {
                    EmployeeId = d.EmployeeId,
                    FirstName = d.FirstName,
                    LastName = d.LastName
                }).ToList(),
                Projects = c.Projects.Select(d => new ProjectDto
                {
                    ProjectId = d.ProjectId,
                    ProjectName = d.ProjectName
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<byte[]> Print()
    {
        var company = await _context.Companies.ToListAsync();

        var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Copanies");

        var headers = new[]
        {
            "Company Name", "Location", "Phone Numner",
            "Founded Yer", "Created Adt", "Updated At"
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

        for (int i = 0; i < company.Count; i++)
        {
            var c = company[i];
            int row = i + 2;
            worksheet.Cells[row, 1].Value = c.Name;
            worksheet.Cells[row, 2].Value = c.Location;
            worksheet.Cells[row, 3].Value = c.PhoneNumber;
            worksheet.Cells[row, 4].Value = c.FoundedYear?.ToString("MMMM dd, yyyy");
            worksheet.Cells[row, 5].Value = c.CreatedAt.ToString("MMMM dd, yyyy");
            worksheet.Cells[row, 6].Value = c.UpdatedAt.ToString("MMMM dd, yyyy");
        }
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        return package.GetAsByteArray();
    }

}
