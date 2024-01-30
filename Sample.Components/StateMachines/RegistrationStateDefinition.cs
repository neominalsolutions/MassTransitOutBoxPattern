namespace Sample.Components.StateMachines;

using MassTransit;


public class RegistrationStateDefinition :
    SagaDefinition<UserRegistrationState>
{
    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator,
        ISagaConfigurator<UserRegistrationState> consumerConfigurator, IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(45), TimeSpan.FromSeconds(60)));
    // yukarýdaki intervallara göre 5 kere deneme yap
      endpointConfigurator.UseMessageRetry(r => r.Immediate(5));

        endpointConfigurator.UseEntityFrameworkOutbox<RegistrationDbContext>(context);
    }
}