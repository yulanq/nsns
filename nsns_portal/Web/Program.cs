using Core.Contexts;
using Core.Interfaces;
using Core.Models;
using Core.Repositories;
using Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Pomelo.EntityFrameworkCore.MySql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add JWT configuration to the dependency injection container
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));


builder.Services.AddScoped<IUserRepository<User>, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Add UserService
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IStaffService, StaffService>();



builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAdminService, AdminService>();


builder.Services.AddScoped<ICoachRepository, CoachRepository>();
builder.Services.AddScoped<ICoachService, CoachService>();


builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();



// Add UserService


// Add password hasher
//builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));

builder.Services.AddScoped<IRepository<City>, CityRepository>();

builder.Services.AddScoped<IRepository<Specialty>, SpecialtyRepository>();

builder.Services.AddDbContext<AppDbContext>(options =>  options.UseMySql( builder.Configuration.GetConnectionString("DefaultConnection"),  new MySqlServerVersion(new Version(8, 0, 39)) // Replace with your MySQL version

    ));


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
    };
});
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//This will ensure any request to the root URL redirects to /User/AddAdmin.
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/Staff/List");
        return;
    }
    await next();
});

app.UseRouting();

app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=User}/{action=AddAdmin}/{id?}");

app.UseEndpoints(endpoints =>
{
    // Map controller routes
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Staff}/{action=List}/{id?}" // Default route points to User/AddAdmin
    );
});

app.UseEndpoints(endpoints =>
{
    // Map controller routes
    endpoints.MapControllerRoute(
        name: "admin_add",
        pattern: "{controller=User}/{action=AddAdmin}/{id?}" // Default route points to User/AddAdmin
    );
});

//app.MapControllerRoute(
//    name: "default",
//    pattern: "User/AddAdmin",
//    defaults: new { controller = "User", action = "AddAdmin" }
//);

//app.MapControllerRoute(
//    name: "staff_list",
//    pattern: "Staff/List",
//    defaults: new { controller = "Staff", action = "List" }
//);



app.Run();
