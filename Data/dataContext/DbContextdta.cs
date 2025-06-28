using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Company.Data.AvtoGenerate.Entity;
using Company.Repository.Employee.Models;
namespace Company.Data.dataContext
{
    public class DbContextdta : DbContext
    {
        public DbContextdta(DbContextOptions<DbContextdta> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("company_schema");


            // Tabelar jamlamasi 
            modelBuilder.Entity<AvtoGenerate.Entity.Companys>().ToTable("companies");
            modelBuilder.Entity<AvtoGenerate.Entity.Department>().ToTable("departments");
            modelBuilder.Entity<AvtoGenerate.Entity.Employee>().ToTable("employees");
            modelBuilder.Entity<AvtoGenerate.Entity.Position>().ToTable("positions");
            modelBuilder.Entity<AvtoGenerate.Entity.Project>().ToTable("projects");
            modelBuilder.Entity<AvtoGenerate.Entity.EmployeeProject>().ToTable("employeeprojects");
            modelBuilder.Entity<AvtoGenerate.Entity.Salarie>().ToTable("salaries");
            modelBuilder.Entity<AvtoGenerate.Entity.Leave>().ToTable("leaves");


        }
        public DbSet<AvtoGenerate.Entity.Companys> Companies { get; set; }
        public DbSet<AvtoGenerate.Entity.Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<EmployeeProject> EmployeeProjects { get; set; }
         public DbSet<Salarie> Salaries { get; set; }
        public DbSet<Leave> Leaves { get; set; }
    }
}
