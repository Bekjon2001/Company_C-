using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Data.AvtoGenerate.Entity;
[Table("salaries", Schema = "company_schema")]
public class Salarie
{
    [Key]
    [Column("salary_id")]
    public int SalaryId { get; set; }

    [Column("employee_id")]
    public int EmployeeId { get; set; }

    [Column("amount", TypeName = "decimal(30,3)")]
    public decimal Amount { get; set; }

    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    [ForeignKey("EmployeeId")]
    public Employee Employee { get; set; }

}
