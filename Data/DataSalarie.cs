using Company.Data.AvtoGenerate.Entity;
using Company.Data.dataContext;
using Company.Dtos;
using Company.Dtos.FilterDto;
using Company.Repository.Salaries;
using Company.Repository.Salaries.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.Data
{
    public class DataSalarie:ISalarieRepositoriy
    {
        private readonly DbContextdta _context;

        public DataSalarie(DbContextdta context)
        {
            _context=context?? throw new ArgumentNullException(nameof(context));
        }

        public int Create(SalarieCreateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var salary = new Salarie
            {
                EmployeeId = dto.EmployeeId,
                Amount = dto.Amount,
                StartDate = dto.StartDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Salaries.Add(salary);
            _context.SaveChanges();

            return salary.SalaryId;
        }

        public bool Update(SalarieUpdateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var salary = _context.Salaries.Find(dto.SalaryId);
            if (salary == null) return false;

            salary.EmployeeId = dto.EmployeeId;
            salary.Amount = dto.Amount;
            salary.StartDate = dto.StartDate;
            salary.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var salary = _context.Salaries.Find(id);
            if (salary == null) return false;

            _context.Salaries.Remove(salary);
            _context.SaveChanges();
            return true;
        }

        public SalarieDto GetById(int id)
        {
            var s = _context.Salaries
                .Include(x => x.Employee)
                .FirstOrDefault(x => x.SalaryId == id);

            if (s == null) return null;

            return new SalarieDto
            {
                SalaryId = s.SalaryId,
                EmployeeId = s.EmployeeId,
                Amount = s.Amount,
                StartDate = s.StartDate,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                EmployeeFullName = s.Employee != null ? $"{s.Employee.FirstName} {s.Employee.LastName}" : null
            };
        }

        public List<SalarieDto> GetAll(SalarieFilterDto filter)
        {
             var query = _context.Salaries
                .Include(x => x.Employee)
                .SortFilter(filter)
                .AsQueryable();

                return query

                .Select(s => new SalarieDto
                {
                    SalaryId = s.SalaryId,
                    EmployeeId = s.EmployeeId,
                    Amount = s.Amount,
                    StartDate = s.StartDate,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    EmployeeFullName = s.Employee != null ? $"{s.Employee.FirstName} {s.Employee.LastName}" : null
                })
                .ToList();
        }

    }
}
