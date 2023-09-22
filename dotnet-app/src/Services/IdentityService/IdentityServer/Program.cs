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

//app.UseHttpsRedirection();
app.MapControllers();
app.UseCors();
app.UseIdentityServer();

await app.InitializeDatabase();

app.Run();
