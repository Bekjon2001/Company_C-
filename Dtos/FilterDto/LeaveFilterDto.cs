namespace Company.Dtos.FilterDto
{
    public class LeaveFilterDto
    {
        public int? LeaveID { get; set; }

        public int PageSize { get; set; }
        public int Page { get; set; }
        public string OrderType { get; set; } = "asc";
    }
}
