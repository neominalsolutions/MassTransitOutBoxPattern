namespace Sample.Components.StateMachines;

using MassTransit;
using Sample.Components.Events;

public class RegistrationStateMachine :
    MassTransitStateMachine<UserRegistrationState>
{
    public RegistrationStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => RegistrationSubmitted, x => x.CorrelateById(m => m.Message.RegistrationId));


    Initially(
        When(RegistrationSubmitted)
            .Then(context =>
            {
              context.Saga.RegistrationDate = context.Message.RegistrationDate;
              context.Saga.UserName = context.Message.Username;
              context.Saga.Email = context.Message.Email;
            })
            .TransitionTo(Registered)
            .ThenAsync(async context =>
            {
              await Console.Out.WriteAsync("Event Store kayýt at");
            })
            .Publish(context => new SendRegistrationEmail
            {
              RegistrationId = context.Saga.CorrelationId,
              RegistrationDate = context.Saga.RegistrationDate,
              Email = context.Saga.Email,
            }).ThenAsync(async context =>
            {
              await Console.Out.WriteAsync("Event Store kayýt at");
            }).Finalize());
               
    }

    //
    // ReSharper disable MemberCanBePrivate.Global
    public State Registered { get; } = null!;
    public Event<RegistrationSubmitted> RegistrationSubmitted { get; } = null!;
}