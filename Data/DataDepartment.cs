using Company.Data.dataContext;
using Company.Dtos;
using Company.Dtos.FilterDto;
using Company.Repository.Departments;
using Company.Repository.Departments.Models;
using Microsoft.EntityFrameworkCore;

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
    }
}
