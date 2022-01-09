using IdentityAuthentication.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(config => 
    {
        config.UseInMemoryDatabase("InMemoryIdentityDB");
    });
builder.Services.AddIdentity<IdentityUser,IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

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
