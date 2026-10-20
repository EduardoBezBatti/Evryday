var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/api/saude", () => Results.Ok(new { status = "API ok" }));

app.Run();

public partial class Program { }
