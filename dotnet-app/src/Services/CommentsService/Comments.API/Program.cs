using Comments.Application.SeedWork;
using Users.Application.SeedWork.Mediator;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();

services.AddCustomDbContext(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining(typeof(Program));

    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
});
services.AddScoped<IDomainEventMediator>(x => new DomainEventMediator(x.GetRequiredService<IServiceScopeFactory>(), typeof(DomainEventMediator)));

services.AddScoped<ICommentQueries>(sp => new CommentQueries(builder.Configuration.GetConnectionString("CommentDB")!));
services.AddScoped<ICommentRepository, CommentRepository>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CommentDbContext>();
    await context.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
    app.Run();
else
    await app.RunAsync();