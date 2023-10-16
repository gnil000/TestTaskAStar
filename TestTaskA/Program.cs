using TestTaskA;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
//app.MapPost("/check", () => { });

app.MapHub<ProcessHub>("/check");
app.Run();
