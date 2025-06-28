using System;

namespace Company.Repository.Company.Models;

public class CompanyUpdateDto
{
    public string Name { get; set; }

    public string Location { get; set; }

    public string PhoneNumber { get; set; }

    public DateTime? FoundedYear { get; set; }
}
