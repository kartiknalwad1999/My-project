using EmployeeServiceDepartmentService.interfaces;
using EmployeeServiceDepartmentService.repos;
using EmployeeServiceDepartmentService.data_access;
//using EmployeeServiceDepartmentService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ✅ Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()                // log to console
    .WriteTo.File("logger/log.txt", rollingInterval: RollingInterval.Day) // log to file
    .CreateLogger();

// Replace default logging with Serilog
builder.Host.UseSerilog();

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Register DbContext with connection string from appsettings.json
builder.Services.AddDbContext<EmpDepartmentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Register generic repository
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));


// ✅ Register MediatR (scan assemblies for handlers)
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(
        Assembly.GetExecutingAssembly()
    ));

var app = builder.Build();

//app.MapDefaultEndpoints();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
