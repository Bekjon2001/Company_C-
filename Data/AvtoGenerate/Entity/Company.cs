using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Data.AvtoGenerate.Entity;

[Table("companies", Schema = "company_schema")]
public class Companys
{
    [Key]
    [Column("company_id")]
    public int CompanyId { get; set; }

    [Required, MaxLength(500)]
    [Column("name")]
    public string Name { get; set; }

    [Required, MaxLength(500)]
    [Column("location")]
    public string Location { get; set; }

    [MaxLength(14)]
    [Column("phone_number")]
    public string? PhoneNumber { get; set; }

    [Column("founded_year")]
    public DateTime? FoundedYear { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    public List<Department>? Departments { get; set; }
    public List<Project>? Projects { get; set; }
    public List<Employee>? Employees { get; set; }
}
