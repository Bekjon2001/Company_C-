namespace Company.Dtos.FilterDto
{
    public class DepartmentFilterDto
    {
        public string? Name { get; set; }
        public int? DepartmentId { get; set; }

        public int PageSize { get; set; }
        public int Page { get; set; }
        public string OrderType { get; set; } = "asc";
    }
}
