using IdentityServer.Extentions;

var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsProduction())
{
    builder.Configuration.AddJsonFile("/etc/secrets/secrets.json", false, true);
}
var config = builder.Configuration;
var services = builder.Services;
services.AddControllers();
if (builder.Environment.IsDevelopment())
{
    services.AddSwaggerGen();
}
services.AddCustomCors(config);
services.AddCustomDbContext(config);
services.AddCustomIdentityServer(config);
services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCookiePolicy(new CookiePolicyOptions()
{
    Secure = CookieSecurePolicy.Always
});
app.MapControllers();
app.UseCors();
app.UseIdentityServer();

await app.InitializeDatabase();

if (app.Environment.IsProduction())
{
    app.MapGet("/", () => $"Identity API Works!");
    var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
    Console.WriteLine($"Port: {port}");
    var url = $"http://0.0.0.0:{port}";
    app.Run(url);
}
else
    app.Run();
