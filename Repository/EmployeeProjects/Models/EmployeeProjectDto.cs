namespace Company.Repository.EmployeeProjects.Models
{
    public class EmployeeProjectDto
    {
        public int EmployeeProjectId {  get; set; }
        public int ProjectId {  get; set; }
        public int EmployeeID { get; set; }
        public string? ProjectName { get; set; }
        public string? EmployeeFullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

       
    }
}
