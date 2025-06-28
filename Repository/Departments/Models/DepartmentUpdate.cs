namespace Company.Repository.Departments.Models;

public class DepartmentUpdate
{
    public int DepartmentId { get; set; }

    public string Name { get; set; }
    public int CompanyId { get; set; }
    public string CompanyName { get; set; }

}
