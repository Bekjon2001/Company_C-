using Bogus;
using Company.Data.AvtoGenerate.Entity;
using Company.Data.dataContext;
using Company.Repository.Employee.Models;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Company.Data.Seeders;

public class DataSeeder
{
    private static object random;

    public static void Seed(DbContextdta dbContextdta)
    {
        if (dbContextdta.Companies.Any()) return; //Ma'lumotlar bor bo‘lsa, yozmasin
        using var transaction = dbContextdta.Database.BeginTransaction();
        try
        {
            var companies = GenerateCompanies(50);
            dbContextdta.Companies.AddRange(companies);
            dbContextdta.SaveChanges();

            var departments = GenerateDepartments(companies);
            dbContextdta.Departments.AddRange(departments);
            dbContextdta.SaveChanges();

            var positions = GeneratePositions(10);
            dbContextdta.Positions.AddRange(positions);
            dbContextdta.SaveChanges();

            var employee = GenerateEmployee(companies, departments, positions);
            dbContextdta.Employee.AddRange(employee);
            dbContextdta.SaveChanges();

            var projects = GenerateProjects(companies);
            dbContextdta.Projects.AddRange(projects);
            dbContextdta.SaveChanges();

            var employeeProjects = GenerateEmployeeProjects(employee, projects);
            dbContextdta.EmployeeProjects.AddRange(employeeProjects);
            dbContextdta.SaveChanges();

            var salaries = GenerateSalaries(employee);
            dbContextdta.Salaries.AddRange(salaries);
            dbContextdta.SaveChanges();

            var leaves = GenerateLeaves(employee);
            dbContextdta.Leaves.AddRange(leaves);
            dbContextdta.SaveChanges();

            // Barcha ma’lumotlar muvaffaqiyatli yozildi — commit qilamiz
            transaction.Commit();
        }
        catch (Exception ex)
        {
            // Xatolik bo‘lsa — rollback (hamma o‘zgarishlarni bekor qil)
            transaction.Rollback();
            Console.WriteLine($"Xatolik: {ex.Message}");
            throw;
        }
    }


    private static List<Companys> GenerateCompanies(int count)
    {
        var companies = new List<Companys>();
        var companyNames = new HashSet<string>();
        var faker = new Faker();

        while (companies.Count < count)
        {
            var name = faker.Company.CompanyName();
            var phone = faker.Phone.PhoneNumber();

            if (companyNames.Add(name)) // Takror bo‘lsa, qo‘shilmaydi
            {
                companies.Add(new Companys
                {
                    Name = name,
                    Location = faker.Address.FullAddress(),
                    PhoneNumber = phone.Length > 14 ? phone.Substring(0, 14) : phone,
                    FoundedYear = faker.Date.Past(30, DateTime.Now.AddYears(-18).ToUniversalTime()),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                });
            }
        }

        return companies;
    }

    private static List<Department> GenerateDepartments(List<Companys> companies) 
    { 
        var result = new List<Department>();
        var  departmentNames = new[] { "IT", "HR", "Marketing", "Finance", "Logistics" };
        var random = new Random();
        var id = 1;
        foreach (var company in companies) 
        {
            for (int i = 0; i < 3; i++) // Masalan, har bir kompaniyaga 3 ta tasodifiy department
            {
                var randomDepartment = departmentNames[random.Next(departmentNames.Length)];
                result.Add(new Department
                {
                    DepartmentId = id++,
                    Name = randomDepartment,
                    CompanyId = company.CompanyId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
        }
        return result;

    }

    private static List<Position> GeneratePositions(int  count)
    {
        var faker = new Faker<Position>()
            .RuleFor(p => p.PositionName, f => f.Name.JobTitle() )
            .RuleFor(p => p.Description, f => f.Lorem.Sentence() )
            .RuleFor(p => p.CreatedAt, f => DateTime.UtcNow)
            .RuleFor(p => p.UpdatedAt, f => DateTime.UtcNow);
        return faker.Generate(count);
    }

    private static List<Employee> GenerateEmployee(List<Companys> companies, List<Department> departments, List<Position> positions)
    {
        var employees = new List<Employee>();
        var id = 1;
        var faker = new Faker();

        foreach (var company in companies)
        {
            var compDepartments = departments.Where(d => d.CompanyId == company.CompanyId).ToList();

            for (int i = 0; i< 500; i++)
            {
                var department = faker.PickRandom(compDepartments);
                var position = faker.PickRandom(positions);
                var phone = faker.Phone.PhoneNumber();
                employees.Add(new Employee
                {
                    EmployeeId = id++,
                    FirstName = faker.Name.FirstName(),
                    LastName = faker.Name.LastName(),
                    Email = faker.Internet.Email(),
                    Phone = phone.Length > 14 ? phone.Substring(0, 14) : phone,
                    Hiredate = faker.Date.Past(5).ToUniversalTime(),
                    DateOfBirth = faker.Date.Past(30, DateTime.Now.AddYears(-18).ToUniversalTime()),
                    Address = faker.Address.FullAddress(),
                    CompanyId = company.CompanyId,
                    DepartmentId = department.DepartmentId,
                    PositionId = position.PositionId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
        }
        return employees;
    }

    private static List<Project> GenerateProjects(List<Companys> companies)
    {
        var projects = new List<Project>();
        var faker = new Faker();
        var id = 1;

        foreach (var company in companies)
        {
            for (int i = 0; i < 50; i++)
            {
                projects.Add(new Project
                {
                    ProjectId = id++,
                    ProjectName = faker.Company.CatchPhrase(),
                    Description = faker.Lorem.Paragraph(),
                    StartDate = faker.Date.Past(2).ToUniversalTime(),
                    EndDate = faker.Date.Future(1).ToUniversalTime(),
                    CompanyId = company.CompanyId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
        }
        return projects;
    }

    private static List<EmployeeProject> GenerateEmployeeProjects(List<Employee> employees,List<Project> projects)    
    {
        var faker = new Faker();
        var result = new List<EmployeeProject>();
        var id = 1;

        foreach(var employee in employees)
        {
            var assignedProjects = faker.PickRandom(projects, faker.Random.Int(1, 5));
            foreach (var project in assignedProjects)
            {
                if(project.CompanyId == employee.CompanyId)
                {
                    result.Add(new EmployeeProject
                    {
                        EmployeeProjectId = id++,
                        EmployeeId = employee.EmployeeId,
                        ProjectId = project.ProjectId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
                
            }
        }
        return result;
    }

    private static List<Salarie> GenerateSalaries(List<Employee> employees)
    {
        var faker = new Faker<Salarie>()
            .RuleFor(s => s.Amount, f=> f.Random.Decimal(1000, 5000))
            .RuleFor(s => s.StartDate, f => f.Date.Recent(25).ToUniversalTime())
            .RuleFor(s => s.CreatedAt, f => DateTime.UtcNow)
            .RuleFor(s => s.UpdatedAt, f => DateTime.UtcNow);

        var result  = new List<Salarie>();
        foreach (var employee in employees)
        {
            var salary = faker.Generate();
            salary.EmployeeId = employee.EmployeeId;
            result.Add(salary);
        }
        return result;
    }

    private static List<Leave> GenerateLeaves(List<Employee> employees)
    {
        var faker = new Faker<Leave>()
            .RuleFor(l => l.Reason, f => f.Lorem.Sentence())
            .RuleFor(l => l.StartDate, f => f.Date.Past(1).ToUniversalTime())
            .RuleFor(l => l.EndDate, (f, l) => l.StartDate.Value.AddDays(f.Random.Int(1, 14)))
            .RuleFor(l => l.CreatedAt, f => DateTime.UtcNow)
            .RuleFor(l => l.UpdatedAt, f => DateTime.UtcNow);

        var result = new List<Leave>();
        var leaveCount = 0;
        foreach (var employee in employees)
        {
            if (leaveCount >= 7500) break;

            var leave = faker.Generate();
            leave.EmployeeId = employee.EmployeeId;
            result.Add(leave);
            leaveCount++;
        }
        return result;
    }
}

