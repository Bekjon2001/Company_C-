namespace Company.Repository.Departments.Models;

public class DepartmentCreateDto
{
    public int DepartmentId { get; set; }

    public string Name { get; set; }
    public int CompanyId { get; set; }
}
