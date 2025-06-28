namespace Company.Repository.Salaries.Models;

public class SalarieCreateDto
{
    public int EmployeeId { get; set; }
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
