using Company.Repository.Employee.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Data.AvtoGenerate.Entity;
[Table("employees", Schema = "company_schema")]

public class Employee
{
    [Key]


    [Column("employee_id")]
    public int EmployeeId { get; set; }

    [Column("first_name")]
    [MaxLength(100)]
    public string FirstName { get; set; }

    [Column("last_name")]
    [MaxLength(100)]
    public string LastName { get; set; }

    [Column("email")]
    [MaxLength(100)]
    public string Email { get; set; }

    [Column("phone")]
    [MaxLength(14)]
    public string Phone { get; set; }

    [Column("hire_date")]
    public DateTime? Hiredate { get; set; }

    [Column("date_of_birth")]
    public DateTime? DateOfBirth { get; set; }

    [Column("address")]
    [MaxLength(1000)]
    public string Address { get; set; }

    [Column("company_id")]
    public int CompanyId { get; set; }

    [Column("department_id")]
    public int DepartmentId { get; set; }

    [Column("position_id")]
    public int PositionId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("CompanyId")]
    public Companys Company { get; set; }

    [ForeignKey ("DepartmentId")]
    public Department Department { get; set; }

    [ForeignKey("PositionId")]
    public Position Position { get; set; }

}
