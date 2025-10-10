using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

string filePath = Path.Combine(Directory.GetCurrentDirectory(), "users.json");

Dictionary<string, string> users = new Dictionary<string, string>();
if (File.Exists(filePath))
{
    string json = File.ReadAllText(filePath);
    users = JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
}

app.MapPost("/register", async (HttpContext context) =>
{
    var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
    var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(requestBody);

    string username = data["username"];
    string password = data["password"];

    if (users.ContainsKey(username))
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("User already exists.");
        return;
    }

    users[username] = password;
    File.WriteAllText(filePath, JsonConvert.SerializeObject(users, Formatting.Indented));

    await context.Response.WriteAsync("User registered successfully.");
});

app.MapPost("/login", async (HttpContext context) =>
{
    var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
    var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(requestBody);

    string username = data["username"];
    string password = data["password"];

    if (users.TryGetValue(username, out string storedPassword) && storedPassword == password)
    {
        await context.Response.WriteAsync("Login successful.");
    }
    else
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Invalid username or password.");
    }
});

app.Run();