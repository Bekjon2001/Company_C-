using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Data.AvtoGenerate.Entity;

[Table("positions", Schema = "company_schema")]
public class Position
{
    [Key]

    [Column("position_id")]
    public int PositionId {  get; set; }

    [Column("position_name")]
    [MaxLength(100)]
    public string PositionName { get; set; }

    [Column("description")]
    [MaxLength(500)]
    public string Description { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

}
