using PowerPulseRestAPI.Data;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Services.Vehicles;
using Microsoft.AspNetCore.Identity;
using PowerPulseRestAPI.Services.Employees;
using PowerPulseRestAPI.Services.Customers;
using PowerPulseRestAPI.Security;
using PowerPulseRestAPI.Data.Models.UsersModels;
using PowerPulseRestAPI.Services.MaterialService;
using PowerPulseRestAPI.Services.Tools;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PowerPulseContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("PowerPulseContext") ??
                throw new InvalidOperationException("Connection string 'PowerPulseContext' not found.")));
// Add services to the container.
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<IToolService, ToolService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddDataProtection();
builder.Services.AddScoped<ISecretProtector, SecretProtector>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<PowerPulseContext>();
//    db.Database.Migrate();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (!app.Environment.IsEnvironment("Docker"))
{
    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();
}

//app.UseHttpsRedirection();



app.MapControllers();

app.Run();
