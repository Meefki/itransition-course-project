using IdentityServer.Extentions;
using IdentityServer4.Extensions;

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

// https://github.com/IdentityServer/IdentityServer4/issues/4535#issuecomment-647084412
app.Use(async (ctx, next) =>
{
    var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Split(';');
    if (urls is not null && urls.Length > 0)
    {
        string scheme = "https";

        string host = CustomApplicationExtentions.FindHttpsUrl(urls, scheme);
        if (string.IsNullOrEmpty(host))
        {
            host = ctx.Request.Host.ToString();
        } 
        else
        {
            ctx.Request.Scheme = scheme;
            ctx.Request.IsHttps = true;
        }
            
        ctx.Request.Host = new HostString(host);
    }
    await next();
});
app.UseIdentityServer();

app.MapGet("/", () => $"Identity API Works!");

await app.InitializeDatabase();
app.Run();
