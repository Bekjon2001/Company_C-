using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Company.Data.AvtoGenerate.Entity
{
    [Table("user_accounts", Schema = "company_schema")]
    public class UserAccount
    {
        [Key]
        [JsonIgnore]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("username")]
        [MaxLength(100)]
        public string Username { get; set; }

        [Column("password_hash")]
        [MaxLength(255)]
        public string Password { get; set; }

        [JsonIgnore]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
