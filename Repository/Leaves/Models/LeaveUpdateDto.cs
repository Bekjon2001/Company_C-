namespace Company.Repository.Leaves.Models;

public class LeaveUpdateDto
{
    public int LeaveId { get; set; }  
    public int EmployeeId { get; set; }  
    public string? Reason { get; set; }  
    public DateTime? StartDate { get; set; }  
    public DateTime? EndDate { get; set; }
}
