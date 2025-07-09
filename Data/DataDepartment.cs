using Company.Data.dataContext;
using Company.Dtos;
using Company.Dtos.FilterDto;
using Company.Repository.Departments;
using Company.Repository.Departments.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Company.Data
{
    public class DataDepartment : IDepartmentRepository
    {
        private readonly DbContextdta _context;

        public DataDepartment(DbContextdta context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public int Create(DepartmentCreateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var departemnt = new AvtoGenerate.Entity.Department
            {
                Name = dto.Name,
                CompanyId = dto.CompanyId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Departments.Add(departemnt);
            _context.SaveChanges();

            return departemnt.Name.Length;
        }

        public DepartmentDto Update(DepartmentUpdate dto, int id)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var department = _context.Departments
                .Include(d => d.Company)
                .FirstOrDefault(d => d.DepartmentId == id);

            if (department == null)
                return null;

            department.Name = dto.Name ?? department.Name;
            department.CompanyId = dto.CompanyId;
            department.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();

            return new DepartmentDto
            {
                DepartmentId = department.DepartmentId,
                Name = department.Name,
                CompanyId = department.CompanyId,
                CompanyName = department.Company?.Name,
                CreatedAt = department.CreatedAt,
                UpdatedAt = department.UpdatedAt
            };
        }


        public bool Delete(int id)
        {

            var department = _context.Departments.Find(id);
            if (department == null) return false;

            _context.Departments.Remove(department);
            _context.SaveChanges();
            return true;
        }

        public DepartmentDto GetById(int id)
        {
            var department = _context.Departments
                .Include(d => d.Company)
                .FirstOrDefault(d => d.DepartmentId == id);

            return new DepartmentDto
            {
                DepartmentId = department.DepartmentId,
                Name = department.Name,
                CompanyId = department.CompanyId,
                CompanyName = department.Company?.Name,
                UpdatedAt = department.UpdatedAt,
                CreatedAt = department.CreatedAt
            };
        }

        public List<DepartmentDto> GetAll(DepartmentFilterDto filter)
        {
            var query = _context.Departments
                .Include(d => d.Company)
                .AsQueryable();

            // ✅ Filter, sort, paging
            query = query.SortFilter(filter);

            return query.Select(c => new DepartmentDto
            {
                DepartmentId = c.DepartmentId,
                Name = c.Name,
                CompanyName = c.Company.Name,
                CompanyId = c.CompanyId,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
            }).ToList();




        }

        public async Task<byte[]> Print()
        {
            var department = await _context.Departments.Include(e => e.Company).ToListAsync();
            
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Department");

            var headers = new[] { "Department Name", "CompanyId", "Company Name", "Created Adt", "Updated At" };

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

            for (int i = 0; i < department.Count;i++)
            {
                var d = department[i];
                int row = i + 2;

                worksheet.Cells[row,1].Value = d.Name;
                worksheet.Cells[row, 2].Value = d.CompanyId;
                worksheet.Cells[row, 3].Value = d.Company?.Name?? "Noma'lum";
                worksheet.Cells[row, 4].Value = d.CreatedAt.ToString("g");
                worksheet.Cells[row,5].Value = d.UpdatedAt.ToString("g");
            }
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            return package.GetAsByteArray();

        }
    }
}
