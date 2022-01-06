var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", config =>
    {
        config.Cookie.Name = "Doctors.Cookie";
        config.LoginPath = "/PatientHealth/Login";
    });

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
app.UseAuthorization();

app.MapGet("/heartbeat", () => $"Claim-based authorization project working : {DateTime.Now.ToLongDateString()}");
app.MapDefaultControllerRoute();
app.MapControllers();

app.Run();
