using Microsoft.EntityFrameworkCore;
using MotelManagement.Business.IService;
using MotelManagement.Business.Service;
using MotelManagement.Common;
using MotelManagement.Core.IRepository;
using MotelManagement.Core.Repository;
using MotelManagement.Data.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
var connectionString = builder.Configuration.GetConnectionString("DbConnection");
builder.Services.AddDbContext<MotelManagementContext>(option =>
{
    option.UseSqlServer(connectionString);
});

builder.Services.AddSession();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Inject 
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<UploadFileUnit>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.UseSession();

app.Run();
