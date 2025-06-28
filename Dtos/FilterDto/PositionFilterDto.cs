namespace Company.Dtos.FilterDto
{
    public class PositionFilterDto
    {
        public int? PositionId { get; set; }
        public string? PositionName { get; set; }

        public int PageSize { get; set; }
        public int Page { get; set; }
        public string OrderType { get; set; } = "asc";
    }
}
