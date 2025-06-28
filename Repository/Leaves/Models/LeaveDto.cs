namespace Company.Repository.Leaves.Models;

public class LeaveDto
{
    public int LeaveID {  get; set; }
    public int EmployeeID {  get; set; }
    public string Reason {  get; set; }
    public string EmployeeName { get; set; }
    public DateTime? StartDate {  get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
