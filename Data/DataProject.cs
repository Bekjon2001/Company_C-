using Company.Data.AvtoGenerate.Entity;
using Company.Data.dataContext;
using Company.Dtos;
using Company.Dtos.FilterDto;
using Company.Repository.Projects;
using Company.Repository.Projects.Models;
using Microsoft.EntityFrameworkCore;

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


}
