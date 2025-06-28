namespace Company.Repository.Projects.Models;

public class ProjectDto
{
    public int ProjectId {  get; set; }
    public string ProjectName { get; set; }
    public string Description {  get; set; }
    public DateTime? StartDate {  get; set; }
    public DateTime? EndDate { get; set; }
    public int CompanyId {  get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Foreign key orqali bog‘langan Company nomi uchun qo‘shimcha property
    public string CompanyName { get; set; }
}
