using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Company.Repository.DtosExxsil
{
    public class UploadExcelDto
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
