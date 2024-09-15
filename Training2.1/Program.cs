using Microsoft.EntityFrameworkCore;
using Training2._1.Data;
using Training2._1.Repo.Implement;
using Training2._1.Repo.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//var provider = builder.Services.BuildServiceProvider();
//var configuration = provider.GetService<IConfiguration>();
builder.Services.AddDbContext<MyDbContext>(o=>o.UseSqlServer(builder.Configuration.GetConnectionString("dbcs")));

builder.Services.AddTransient<IStudentRepo, StudentRepo>();
builder.Services.AddTransient<ITeacherRepo, TeacherRepo>();
builder.Services.AddTransient<IDepartmentRepo, DepartmentRepo>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Student}/{action=Index}/{id?}");

app.Run();
