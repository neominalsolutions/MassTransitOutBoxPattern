
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample.Components;
using Sample.Components.Consumers;
using Sample.Components.StateMachines;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("MassTransit", LogEventLevel.Debug)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
      services.AddDbContext<RegistrationDbContext>(x =>
      {

        x.UseNpgsql(hostContext.Configuration.GetConnectionString("Default"), options =>
        {
          options.MinBatchSize(1);
        });
      });


      services.AddMassTransit(x =>
      {
        x.AddEntityFrameworkOutbox<RegistrationDbContext>(o =>
        {
          o.UsePostgres();

          o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
        });

        x.SetKebabCaseEndpointNameFormatter();

        x.AddConsumer<RegisterationSubmittedConsumer>();
        x.AddConsumer<SendRegistrationEmailConsumer>();
        // Saga State Machine'i RegistrationStateDefinition ayarları ile Register ettik. (Resilency sağladık. hata durumlarına karşı direç)
        x.AddSagaStateMachine<RegistrationStateMachine, UserRegistrationState, RegistrationStateDefinition>()
            .EntityFrameworkRepository(r =>
            {
              r.ExistingDbContext<RegistrationDbContext>();
              r.UsePostgres();
            });

        x.UsingRabbitMq((context, cfg) =>
        {
          cfg.ConfigureEndpoints(context);
        });
      });
    })
    .UseSerilog()
    .Build();

await host.RunAsync();
