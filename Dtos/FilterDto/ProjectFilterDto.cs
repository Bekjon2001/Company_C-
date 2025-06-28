namespace Company.Dtos.FilterDto
{
    public class ProjectFilterDto
    {
        public int? ProjectId { get; set; }
        public string? ProjectName { get; set; }

        public int PageSize { get; set; }
        public int Page { get; set; }
        public string OrderType { get; set; } = "asc";
        
    }
}
