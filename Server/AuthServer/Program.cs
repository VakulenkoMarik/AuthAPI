var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

var app = builder.Build();

app.UseRouting();
app.MapControllers();

app.Run("http://localhost:5000");