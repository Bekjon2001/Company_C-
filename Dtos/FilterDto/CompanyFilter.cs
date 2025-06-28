namespace Company.Dtos.FilterDto
{
    public class CompanyFilterDto
    {

        public string? CompanyName {  get; set; }
        public int? CompanyId { get; set; }
        public int PageSize { get; set; }
        public int Page {  get; set; }
        public string OrderType { get; set; } = "asc";
    }
}
