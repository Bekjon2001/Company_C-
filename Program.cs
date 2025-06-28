using Company.Data;
using Company.Data.AvtoGenerate.Entity;
using Company.Data.dataContext; // DbContextdta shu namespace da
using Company.Data.Seeders;
using Company.Repository.Company;
using Company.Repository.Departments;
using Company.Repository.Employee;
using Company.Repository.EmployeeProjects;
using Company.Repository.Positions;
using Microsoft.EntityFrameworkCore;
using StackExchange.Profiling;
using Microsoft.OpenApi.Models;
using web_application;
using Serilog;
using System.Reflection;
using Company.Converters;
using Company.Repository.Leaves;
using Company.Repository.Projects;
using Company.Repository.Salaries;

namespace Company
{
    public class Program
    {
        private static object c;

        public static object MiniProfilerEFLoggerFactory { get; private set; }

        public static void Main(string[] args)
        {
            // Serilog konfiguratsiyasi 
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Seq("http://localhost:5341") // agar Seq o‘rnatilgan bo‘lsa
                .CreateLogger();


            var builder = WebApplication.CreateBuilder(args);

            //Json serializer’dan
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
                });

            //JSON serializationda format berish
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                });

            // Serilog'ni ASP.NET Core hosting'ga biriktirish
            builder.Host.UseSerilog();


            // AppSettings ni DI orqali sozlash
            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

            // Add services to the container.
            builder.Services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
                        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                    });
            builder.Services.AddEndpointsApiExplorer();

            // MiniProfiler
            builder.Services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/profiler";
                options.TrackConnectionOpenClose = true;
            }).AddEntityFramework();

            // Swagger
            builder.Services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Company API",
                    Version = "v1",
                    Description = "API for managing company data"
                });

                // XML hujjatlarni qo‘shish
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);

                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
                else
                {
                    Console.WriteLine($"Warning: XML documentation file '{xmlPath}' not found.");
                }
            });

            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
           
        
            var constr = builder.Configuration.GetValue<string>("AppSettings:Postgres:ConnectionString");


            builder.Services.AddDbContext<DbContextdta>(options =>
            options.UseNpgsql(constr)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors());
            //.UseLoggerFactory(MiniProfilerEFLoggerFactory.Current));

            // Repository registratsiyasi
            builder.Services.AddScoped<ICopanyRepositoriy, DataCompany>();
            builder.Services.AddScoped<IDepartmentRepository, DataDepartment>();
            builder.Services.AddScoped<IPositionRepository, DataPosition>();
            builder.Services.AddScoped<IEmployeeProjectRepository, DataEmployeeProject>();
            builder.Services.AddScoped<IEmployeeRepositoriy, DataEmployee>();
            builder.Services.AddScoped<ILeaveRepositrory, DataLeave>();
            builder.Services.AddScoped<IPositionRepository, DataPosition>();
            builder.Services.AddScoped<IProjectRepository, DataProject>();
            builder.Services.AddScoped<ISalarieRepositoriy, DataSalarie>();


            // CORS sozlamalari
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Swagger faqat development rejimida
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Xato boshqaruvi
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); 
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //app.UseExceptionHandler(errorApp =>
            //{
            //    errorApp.Run(async context =>
            //    {
            //        context.Response.StatusCode = 500;
            //        context.Response.ContentType = "application/json";
            //        await context.Response.WriteAsync("{\"error\": \"An unexpected error occurred.\"}");
            //    });
            //});

            app.UseHttpsRedirection();
            app.UseMiniProfiler();
            app.UseCors("AllowAll");
            app.UseAuthorization();
            app.MapControllers();

            // Seed qilish (ma'lumotlarni avto-to‘ldirish)
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DbContextdta>();
                DataSeeder.Seed(dbContext);
            }
            app.Run();


        }
    }
}

