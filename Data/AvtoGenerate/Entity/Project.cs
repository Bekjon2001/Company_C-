using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Data.AvtoGenerate.Entity;
[Table("projects", Schema = "company_schema")]

public class Project
{
    [Key]
    [Column("project_id")]
    public int ProjectId { get; set; }

    [Column("project_name")]
    [MaxLength(20000)]
    public string ProjectName { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("start_date")]
    public DateTime? StartDate { get; set; }

    [Column("ent_date")]
    public DateTime? EndDate { get; set; }

    [Column("company_id")]
    public int CompanyId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    // Foreign key relationship
    [ForeignKey("CompanyId")]
    public Companys Company { get; set; }
}
