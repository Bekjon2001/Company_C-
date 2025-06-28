using Company.Repository.Departments.Models;
using Company.Repository.Employee.Models;
using Company.Repository.Projects.Models;
using System.ComponentModel.DataAnnotations;

namespace Company.Repository.Company.Models
{
    public class CompanyDto
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? FoundedYear { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Forenkiy ulanga tabelar 
        public List<DepartmentDto> Departments { get; set; }
        public List<EmployeeDto> Employees { get; set; }
        public List<ProjectDto> Projects { get; set; }
        
    }
}
