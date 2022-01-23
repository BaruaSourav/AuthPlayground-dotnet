using System.Security.Claims;
using IdentityAuthentication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(config => 
    {
        config.UseInMemoryDatabase("InMemoryIdentityDB");
    });
builder.Services.AddIdentity<IdentityUser,IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "Clinic.Cookie";
    config.LoginPath = "/Account/Login";
});

builder.Services.AddAuthorization(config =>
{
    var authBuilder = new AuthorizationPolicyBuilder();
    config.DefaultPolicy = authBuilder
    .RequireAuthenticatedUser()
    .RequireClaim(ClaimTypes.DateOfBirth)
    .Build();
});
//Mailkit configuration
var mailKitOpt = builder.Configuration.GetSection("Email").Get<MailKitOptions>();
builder.Services.AddMailKit(config => {
    config.UseMailKit(mailKitOpt);
});

//Configure
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    options.SignIn.RequireConfirmedAccount = true;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});
// builder.Services.AddAuthentication("CookieAuth")
//     .AddCookie("CookieAuth", config =>
//     {
//         config.Cookie.Name = "Clinic.Cookie";
//         config.LoginPath = "/Account/Login";
//     });


var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/heartbeat", () => $"Identity Authentication project up and running : {DateTime.Now.ToLongDateString()}");
app.MapDefaultControllerRoute();
app.MapControllers();

app.Run();
