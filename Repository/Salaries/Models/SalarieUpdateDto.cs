namespace Company.Repository.Salaries.Models;

public class SalarieUpdateDto
{
    public int SalaryId { get; set; }
    public int EmployeeId { get; set; }
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
}
