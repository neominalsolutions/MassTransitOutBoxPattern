using MassTransit;
using Microsoft.EntityFrameworkCore;
using Sample.Api;
using Sample.API.Contexts;
using Sample.API.Services;
using Sample.Components;
using Serilog;
using Serilog.Events;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("MassTransit", LogEventLevel.Debug)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddScoped<IRegistrationService, RegistrationService>();

//builder.Services.AddDbContext<SampleAPIDbContext>(x =>
//{
//  x.UseNpgsql(builder.Configuration.GetConnectionString("Default"), options =>
//  {

//    options.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
//    options.MigrationsHistoryTable($"__{nameof(SampleAPIDbContext)}");

//    options.EnableRetryOnFailure(5);
//    options.MinBatchSize(1);
//  });
//});

//builder.Services.AddHostedService<RecreateDatabaseHostedService<SampleAPIDbContext>>();

builder.Services.AddDbContext<RegistrationDbContext>(x =>
{
  x.UseNpgsql(builder.Configuration.GetConnectionString("Outbox"), options =>
  {
    options.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
    options.MigrationsHistoryTable($"__{nameof(RegistrationDbContext)}");

    options.EnableRetryOnFailure(5);
    options.MinBatchSize(1);
  });
});


builder.Services.AddHostedService<RecreateDatabaseHostedService<RegistrationDbContext>>();

builder.Services.AddMassTransit(x =>
{
  x.AddEntityFrameworkOutbox<RegistrationDbContext>(o =>
  {
    o.QueryDelay = TimeSpan.FromSeconds(1);

    o.UsePostgres();
    o.UseBusOutbox();
  });

  x.UsingRabbitMq((_, cfg) =>
  {
    cfg.AutoStart = true;
  });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
