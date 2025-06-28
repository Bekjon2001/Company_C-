using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Data.AvtoGenerate.Entity;
[Table("leaves", Schema = "company_schema")]

public class Leave
{
    [Key]
    [Column("leave_id")]
    public int LeaveId { get; set; }

    [Column("employee_id")]
    public int EmployeeId { get; set; }

    [Column("reason")]
    [MaxLength(10000)]
    public string Reason { get; set; }

    [Column("start_date")]
    public DateTime? StartDate { get; set; }

    [Column("ent_date")]
    public DateTime? EndDate { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    [ForeignKey("EmployeeId")]
    public Employee Employee { get; set; }
}
