namespace Company.Dtos.FilterDto;

public class EmployeeProjectFilterDto
{
    public int? EmployeeProjectId { get; set; }

    public  string? ProjectName { get; set; }
    public string? EmployeeFullName { get; set; }

    public int PageSize { get; set; }
    public int Page { get; set; }
    public  string OrderType { get; set; } = "asc";
}
