
namespace Company.API.EmployeeControllers
{
    internal class ExcelPackage
    {
        private MemoryStream stream;

        public ExcelPackage(MemoryStream stream)
        {
            this.stream=stream;
        }
    }
}