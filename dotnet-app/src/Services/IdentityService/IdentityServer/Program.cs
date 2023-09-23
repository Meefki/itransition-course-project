using IdentityServer.Extentions;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureEndpointDefaults(lo => lo.Protocols = HttpProtocols.Http2);
});
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

//app.UseHttpsRedirection();
app.MapControllers();
app.UseCors();
app.UseIdentityServer();

await app.InitializeDatabase();

if (app.Environment.IsProduction())
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
    var url = $"http://0.0.0.0:{port}";
    app.Run();
}
else
    app.Run();
