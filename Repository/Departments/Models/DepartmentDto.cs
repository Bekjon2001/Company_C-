using System.Text.Json.Serialization;

namespace Company.Repository.Departments.Models;

public class DepartmentDto
{
    public int DepartmentId { get; set; }

    public string Name { get; set; }
    public string CompanyName { get; set; }
    public int CompanyId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // JSON chiqishi uchun formatlangan string property
    [JsonIgnore]
    public string CreatedAtFormatted => CreatedAt.ToString("yyyy-MM-dd");
    [JsonIgnore]
    public string UpdatedAtFormatted => UpdatedAt.ToString("yyyy-MM-dd");
}
