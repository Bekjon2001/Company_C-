using Company.Data.AvtoGenerate.Entity;
using Company.Data.dataContext;
using Company.Dtos;
using Company.Dtos.FilterDto;
using Company.Repository.Leaves;
using Company.Repository.Leaves.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Company.Data;

public class DataLeave : ILeaveRepositrory
{
    private readonly DbContextdta _context;

    public DataLeave(DbContextdta context)
    { 
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public int Create(LeaveCreateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var leave = new Leave
        {
            EmployeeId = dto.EmployeeId,
            Reason = dto.Reason,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Leaves.Add(leave);
        _context.SaveChanges();
        return leave.LeaveId;
    }

    public bool Update(LeaveUpdateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var leave = _context.Leaves.Find(dto.LeaveId);
        if (leave == null) return false;

        leave.Reason = dto.Reason ?? leave.Reason;
        leave.StartDate = dto.StartDate ?? leave.StartDate;
        leave.EndDate = dto.EndDate ?? leave.EndDate;
        leave.UpdatedAt = DateTime.UtcNow;

        _context.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var leave = _context.Leaves.Find(id);
        if (leave == null) return false;

        _context.Leaves.Remove(leave);
        _context.SaveChanges();
        return true;
    }

    public LeaveDto GetById(int id)
    {
        var leave = _context.Leaves
            .Include(l => l.Employee)
            .FirstOrDefault(l => l.LeaveId == id);

        if (leave == null) return null;

        return new LeaveDto
        {
            LeaveID = leave.LeaveId,
            EmployeeID = leave.EmployeeId,
            Reason = leave.Reason,
            StartDate = leave.StartDate,
            EndDate = leave.EndDate,
            CreatedAt = leave.CreatedAt,
            UpdatedAt = leave.UpdatedAt,
            EmployeeName = leave.Employee != null
                ? $"{leave.Employee.FirstName} {leave.Employee.LastName}"
                : null
        };
    }

    public List<LeaveDto> GetAll( LeaveFilterDto filter )
    {
        var query = _context.Leaves
            .Include(l => l.Employee)
            .SortFliter(filter)
            .AsQueryable();
        return query
            .Select(leave => new LeaveDto
            {
                LeaveID = leave.LeaveId,
                EmployeeID = leave.EmployeeId,
                Reason = leave.Reason,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                CreatedAt = leave.CreatedAt,
                UpdatedAt = leave.UpdatedAt,
                EmployeeName = leave.Employee != null
                    ? $"{leave.Employee.FirstName} {leave.Employee.LastName}"
                    : null
            })
            .ToList();
    }

    public async Task<byte[]> Print()
    {
        var leave = await _context.Leaves.
             Include(e => e.Employee)
            .ToListAsync();

        var package = new ExcelPackage();

        var worksheet = package.Workbook.Worksheets.Add("Leave");
        var headers = new[]
        {
                "LeaveId", "EmployeeId","Full Name","Reason",
                "Star Date","Ent Date","Created Adt", "Updated At"
        };
        for(int i  = 0; i < headers.Length; i++)
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
        for(int i = 0; i < leave.Count; i++)
        {
            var l = leave[i];
            var row = i +2;
            worksheet.Cells[row, 1].Value = l.LeaveId;
            worksheet.Cells[row, 2].Value = l.EmployeeId;
            worksheet.Cells[row, 3].Value = l.Employee?.FirstName + " " + l.Employee?.LastName;
            worksheet.Cells[row, 4].Value = l.Reason;
            worksheet.Cells[row, 5].Value = l.StartDate?.ToString("g");
            worksheet.Cells[row,6].Value = l.EndDate?.ToString("g");
            worksheet.Cells[row,7].Value = l.CreatedAt.ToString("g");
            worksheet.Cells[row,8].Value = l.UpdatedAt.ToString("g");

        }
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        return package.GetAsByteArray();
    }

}


