using Company.Data.AvtoGenerate.Entity;
using Company.Data.dataContext;
using Company.Dtos;
using Company.Dtos.FilterDto;
using Company.Repository.EmployeeProjects;
using Company.Repository.EmployeeProjects.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Company.Data
{
    public class DataEmployeeProject : IEmployeeProjectRepository
    {
        private readonly DbContextdta _context;

        public DataEmployeeProject(DbContextdta context)
        {
            _context=context ?? throw new ArgumentNullException(nameof(context));
        }
        public int Create(EmployeeProjectCreateDto dto)
        {
            var employeeProject = new EmployeeProject
            {
                ProjectId = dto.ProjectId,
                EmployeeId = dto.EmployeeId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.EmployeeProjects.Add(employeeProject);
            _context.SaveChanges();
            return employeeProject.EmployeeProjectId;
        }

        public bool Update(EmployeeProjectUpdateDto dto)
        {
            var employeeProject = _context.EmployeeProjects.Find(dto.EmployeeProjectId);
            if (employeeProject == null) return false;

            employeeProject.ProjectId = dto.ProjectId;
            employeeProject.EmployeeId = dto.EmployeeId;
            employeeProject.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var employeeProject = _context.EmployeeProjects.Find(id);
            if (employeeProject == null) return false;

            _context.EmployeeProjects.Remove(employeeProject);
            _context.SaveChanges();
            return true;
        }
        public EmployeeProjectDto GetById(int id)
        {
            var ep = _context.EmployeeProjects
                .Include(e => e.Employee)
                .Include(p => p.Project)
                .FirstOrDefault(e => e.EmployeeProjectId == id);

            if (ep == null) return null;

            return new EmployeeProjectDto
            {
                EmployeeProjectId = ep.EmployeeProjectId,
                ProjectId = ep.ProjectId,
                EmployeeID = ep.EmployeeId,
                ProjectName = ep.Project?.ProjectName,
                EmployeeFullName = ep.Employee?.FirstName + " " + ep.Employee?.LastName,
                CreatedAt = ep.CreatedAt,
                UpdatedAt = ep.UpdatedAt,
                
               
            };
        }

        public List<EmployeeProjectDto> GetAll( EmployeeProjectFilterDto filter )
        {
            var query = _context.EmployeeProjects
                .Include(e => e.Employee)
                .Include(p => p.Project)
                .SortFiter(filter)
                .AsQueryable();

            return query
                .Select(ep => new EmployeeProjectDto
                {
                    EmployeeProjectId = ep.EmployeeProjectId,
                    ProjectId = ep.ProjectId,
                    EmployeeID = ep.EmployeeId,
                    CreatedAt = ep.CreatedAt,
                    UpdatedAt = ep.UpdatedAt,
                    ProjectName = ep.Project != null ? ep.Project.ProjectName : null,
                    EmployeeFullName = ep.Employee != null
            ? ep.Employee.FirstName + " " + ep.Employee.LastName
            : null
                }).ToList();
        }

        public async Task<byte[]> Print()
        {
            var emloyeeProject = await _context.EmployeeProjects
                .Include(e => e.Employee)
                .Include(p => p.Project)
                .ToListAsync();


            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("EmployeeProject");

            var headers = new[]
            {
               "EmployeeProjectId", "ProjectId", "Project Name","EmployeeId","Full Name"
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

            for (int i = 0;i < emloyeeProject.Count; i++)
            {
                var e = emloyeeProject[i];
                var row = i + 2;

                worksheet.Cells[row, 1].Value = e.EmployeeProjectId;
                worksheet.Cells[row, 2].Value = e.ProjectId;
                worksheet.Cells[row, 3].Value = e.Project?.ProjectName?? "Noma'lum";
                worksheet.Cells[row, 4].Value = e.EmployeeId;
                worksheet.Cells[row, 5].Value = e.Employee?.FirstName + " " + e.Employee.LastName;

            }
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            return package.GetAsByteArray();

        }
    }
}
