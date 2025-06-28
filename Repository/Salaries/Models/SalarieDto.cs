namespace Company.Repository.Salaries.Models;

public class SalarieDto
{
    public int SalaryId { get; set; }
    public int EmployeeId {  get; set; }
    public decimal Amount {  get; set; }
    public DateTime? StartDate {  get; set; } // sanda tushi
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string EmployeeFullName { get; set; }
    public string EmployeeDepartment { get; set; }
}
