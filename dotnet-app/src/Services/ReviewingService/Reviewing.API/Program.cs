var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCustomDbContext(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining(typeof(Program));

    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
});
builder.Services.AddScoped<IDomainEventMediator>(x => new DomainEventMediator(x.GetRequiredService<IServiceScopeFactory>(), typeof(DomainEventMediator)));

builder.Services.AddScoped<ICommentQueries>(sp => new CommentQueries(builder.Configuration.GetConnectionString("ReviewingDb")!));
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

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
    var context = scope.ServiceProvider.GetRequiredService<ReviewingDbContext>();
    await context.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
    app.Run();
else
    await app.RunAsync();