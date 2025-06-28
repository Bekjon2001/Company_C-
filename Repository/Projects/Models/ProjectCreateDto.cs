namespace Company.Repository.Projects.Models;

public class ProjectCreateDto
{
    public int ProjecID { get; set; }
    public string ProjectName { get; set; }
    public string Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int CompanyId { get; set; }
}
