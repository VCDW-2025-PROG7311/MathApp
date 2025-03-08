using MathApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);


Console.WriteLine(Environment.GetEnvironmentVariable("Math_DB"));
// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddDbContext<MathDbContext>(options =>
//                options.UseSqlServer(builder.Configuration.GetConnectionString("Math_DB")));

builder.Services.AddDbContext<MathDbContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("Math_DB")));

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
    pattern: "{controller=Math}/{action=Calculate}/{id?}");

app.Run();
