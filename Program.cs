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
using Company.Mapping;
using AutoMapper;
using OfficeOpenXml;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Company.Service.Atuh;
using Company.Dost;
//using OfficeOpenXml.LicenseContext; // 🔹 Litsenziya context uchun

namespace Company
{
    public class Program
    {
        private static object c;

        public static object MiniProfilerEFLoggerFactory { get; private set; }

        public static void Main(string[] args)
        {

            var hashed = PasswordHashHandlar.HashPaswword("123");
            Console.WriteLine(hashed);
            // Serilog konfiguratsiyasi 
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Seq("http://localhost:5341") 
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            //Json serializer’dan
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // 1. Default qiymatlarni yozmaslik
                    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;

                    // 2. DateOnly formatni qo‘llash
                    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                });

            // Serilog'ni ASP.NET Core hosting'ga biriktirish
            builder.Host.UseSerilog();

            //AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

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

                // ✅ JWT Bearer token uchun konfiguratsiya
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT token kiriting. Format: Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                // XML hujjatlarni qo‘shish
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
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
            builder.Services.AddScoped<JwtService>();


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

            //EPPlus 8.x litsenziyasini o‘rnatish
            //ExcelPackage.License = new EPPlusLicense("NonCommercial");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //JwtBearer sozlamalari
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JwtConfig:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JwtConfig:Audience"],

                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                                         Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]))
                };
            });
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Swagger faqat development rejimida
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
            });

            // Xato boshqaruvi
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

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


