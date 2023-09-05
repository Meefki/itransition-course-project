var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsProduction())
{
    builder.Configuration.AddUserSecrets(typeof(Program).Assembly);
}
var config = builder.Configuration;

var services = builder.Services;
services.AddControllers();
//builder.Services.AddDefaultOpenApi(config);
services.AddDefaultAuthentication(config);
if (builder.Environment.IsDevelopment())
{
    services.AddSwaggerGen();
}
services.AddCustomDbContext(config);
services.AddCustomCors(config);
services.AddEndpointsApiExplorer();
services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining(typeof(Program));

    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
});
services.AddScoped<IDomainEventMediator>(x => new DomainEventMediator(x.GetRequiredService<IServiceScopeFactory>(), typeof(DomainEventMediator)));
services.AddScoped<ICommentQueries>(sp => new CommentQueries(config.GetConnectionString("Reviewving")!));
services.AddScoped<IReviewQueries>(sp => new ReviewQueries(config.GetConnectionString("Reviewving")!));
services.AddScoped<ICommentRepository, CommentRepository>();
services.AddScoped<IReviewRepository, ReviewRepository>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ReviewingDbContext>();
    await context.Database.MigrateAsync();
}

DapperSetup.Init();

if (app.Environment.IsDevelopment())
    app.Run();
else
    await app.RunAsync();