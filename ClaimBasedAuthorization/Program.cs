var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();

app.MapGet("/", () => "Claim-based authorization project working");

app.Run();
