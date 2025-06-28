namespace Company.Repository.EmployeeProjects.Models;

public class EmployeeProjectCreateDto
{
    public int ProjectId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
