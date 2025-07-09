using Company.Data.AvtoGenerate.Entity;
using Company.Data.dataContext;
using Company.Dtos;
using Company.Dtos.FilterDto;
using Company.Repository.Projects;
using Company.Repository.Projects.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Company.Data;

public class DataProject:IProjectRepository
{
    private readonly DbContextdta _context;

    public DataProject(DbContextdta context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public int Create(ProjectCreateDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        var project = new Project
        {
            ProjectName = dto.ProjectName,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            CompanyId = dto.CompanyId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        _context.SaveChanges();

        return project.ProjectId;
    }

    public bool Update(ProjectUpdateDto dto, int id)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == dto.ProjectId);
        if (project == null) return false;

        project.ProjectName = dto.ProjectName ?? project.ProjectName;
        project.Description = dto.Description ?? project.Description;
        project.StartDate = dto.StartDate ?? project.StartDate;
        project.EndDate = dto.EndDate ?? project.EndDate;
        project.CompanyId = dto.CompanyId;
        project.UpdatedAt = DateTime.UtcNow;

        _context.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var project = _context.Projects.Find(id);
        if (project == null) return false;

        _context.Projects.Remove(project);
        _context.SaveChanges();
        return true;
    }

    public ProjectDto GetById(int id)
    {
        var project = _context.Projects
            .Include(p => p.Company)
            .FirstOrDefault(p => p.ProjectId == id);

        if (project == null) return null;

        return new ProjectDto
        {
            ProjectId = project.ProjectId,
            ProjectName = project.ProjectName,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            CompanyId = project.CompanyId,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt,
            CompanyName = project.Company?.Name // foreign key orqali bog‘langan nom
        };
    }

    public List<ProjectDto> GetAll(ProjectFilterDto filter)
    {
        var projects = _context.Projects
            .Include(p => p.Company)
            .SortFilter(filter)
            .ToList();

        var result = projects.Select(project => new ProjectDto
        {
            ProjectId = project.ProjectId,
            ProjectName = project.ProjectName,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            CompanyId = project.CompanyId,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt,
            CompanyName = project.Company?.Name
        }).ToList();

        return result;
    }

    public async Task<byte[]> Print()
    {
        var project = await _context.Projects
            .Include(p => p.Company)
            .ToListAsync();

        var package = new ExcelPackage();

        var worksheet = package.Workbook.Worksheets.Add("Project");

        var headers = new[]
        {
            "ProjectId", "Project Namne", "Project Description",
            "Start Ad", "Ent Ad", "CompanyId", "Company Name",
            "Created Adt", "Updated At"
        };
        for(int i = 0; i < headers.Length; i++)
        {
            worksheet.Cells[1, i +1].Value = headers[i];
        }
            using (var range = worksheet.Cells[1, 1, 1, headers.Length])
            {
                range.Style.Font.Name = "Arial Black";
                range.Style.Font.Size = 11;
                range.Style.Font.Bold = true;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.WrapText = false;
            }

        for (int i = 0; i < project.Count; ++i)
        {
            var p = project[i];
            var row = i + 2;
            worksheet.Cells[row, 1].Value = p.ProjectId;
            worksheet.Cells[row, 2].Value = p.ProjectName;
            worksheet.Cells[row, 3].Value = p.Description;
            worksheet.Cells[row, 4].Value = p.StartDate?.ToString("g");
            worksheet.Cells[row, 5].Value = p.EndDate?.ToString("g");
            worksheet.Cells[row, 6].Value = p.CompanyId;
            worksheet.Cells[row, 7].Value = p.Company?.Name;
            worksheet.Cells[row, 8].Value = p.CreatedAt.ToString("g");
            worksheet.Cells[row,9].Value = p.UpdatedAt.ToString("g");
        }
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        return package.GetAsByteArray();
    }


}
