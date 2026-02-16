using EvidenciaStudentov.Application.Features.Student.Queries;
using EvidenciaStudentov.Application.Features.Ucitel.Commands;
using EvidenciaStudentov.Application.Features.Ucitel.Queries;
using EvidenciaStudentov.Infrastructure.Authorization;
using EvidenciaStudentov.Infrastructure.Persistence;
using EvidenciaStudentov.Shared.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 28))));

builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<AuthFilter>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });

builder.Services.AddAppAuthorization();

builder.Services.AddScoped<IUcitelQueryService, UcitelQueryService>();
builder.Services.AddScoped<IUcitelCommandService, UcitelCommandService>();
builder.Services.AddScoped<IStudentQueryService, StudentQueryService>();


builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

await DbSeeder.SeedDefaultUsersAsync(app.Services);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

