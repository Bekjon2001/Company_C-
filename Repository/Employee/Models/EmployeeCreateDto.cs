namespace Company.Repository.Employee.Models;

public class EmployeeCreateDto
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime? Hiredate { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Address { get; set; }
    public int CompanyId { get; set; }
    public int DepartmentId { get; set; }
    public int PositionId { get; set; }
}
