namespace Company.Dtos.FilterDto;


public class EmployeeFilterDto
{
    public int? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int PageSize { get; set; }
    public int Page { get; set; }
    public string OrderType { get; set; } = "asc";
}
