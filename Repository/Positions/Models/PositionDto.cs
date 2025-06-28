namespace Company.Repository.Positions.Models;

public class PositionDto
{
    public int PositionId { get; set; }

    public string PositionName {  get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
