using Company.Data.AvtoGenerate.Entity;
using Company.Data.dataContext;
using Company.Dtos;
using Company.Dtos.FilterDto;
using Company.Repository.Salaries;
using Company.Repository.Salaries.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

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

        public async Task<byte[]> Print()
        {
            var salarie = await _context.Salaries.
                Include(x => x.Employee)
                .ToListAsync();

            var package = new ExcelPackage();

            var worksheet = package.Workbook.Worksheets.Add("Salarie");

            var headers = new[]
            {
                "SalariId","EployeeId","Full Name","Amount", "Start Ad",
                "Created Adt", "Updated At"
            };
            for (int i = 0; i < headers.Length; i++)
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

            for (int i = 0; i < salarie.Count; i++)
            {
                var s = salarie[i];
                var row = i + 2;

                worksheet.Cells[row, 1].Value = s.SalaryId;
                worksheet.Cells[row, 2].Value = s.EmployeeId;
                worksheet.Cells[row, 3].Value = s.Employee?.FirstName + " " + s.Employee?.LastName;
                worksheet.Cells[row, 4].Value = s.Amount;
                worksheet.Cells[row, 5].Value = s.StartDate.ToString("g");
                worksheet.Cells[row, 6].Value = s.CreatedAt.ToString("g");
                worksheet.Cells[row, 7].Value = s.UpdatedAt.ToString("g");
            }
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            return package.GetAsByteArray();

        }

    }
}
