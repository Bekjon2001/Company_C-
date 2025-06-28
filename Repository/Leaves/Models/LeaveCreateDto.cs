namespace Company.Repository.Leaves.Models;

public class LeaveCreateDto
{
    public int EmployeeId { get; set; }  // Fixed casing to match usage
    public string Reason { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
