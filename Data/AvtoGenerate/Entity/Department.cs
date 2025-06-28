using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Data.AvtoGenerate.Entity;

[Table("departments", Schema = "company_schema")]
public class Department
{
    [Key]

    [Column("department_id")]
    public int DepartmentId { get; set; }

    [Column("name")]
    [Required, MaxLength(200)]
    public string Name { get; set; }

    [Column("company_id")]
    public int CompanyId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey(nameof(CompanyId))]
    public Companys Company { get; set; }

}
