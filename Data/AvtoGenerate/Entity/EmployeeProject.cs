using Company.Data.AvtoGenerate.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Data.AvtoGenerate.Entity;

[Table("employeeprojects", Schema = "company_schema")]

public class EmployeeProject
{
    [Key]
    [Column("employee_projects_id")]
    public int EmployeeProjectId { get; set; }

    [Column("projects_id")]
    public int ProjectId { get; set; }

    [Column("employee_id")]
    public int EmployeeId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("ProjectId")]
    public Project Project { get; set; }

    [ForeignKey("EmployeeId")]
    public Employee Employee { get; set; }
}
