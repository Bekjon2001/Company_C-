namespace Company.Dtos.FilterDto
{
    public class SalarieFilterDto
    {
        public int? SalaryId { get; set; }

        public int PageSize { get; set; }
        public int Page { get; set; }
        public string OrderType { get; set; } = "asc";

        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }

    }
}
