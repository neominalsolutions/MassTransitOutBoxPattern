namespace Sample.Components.StateMachines;

using MassTransit;


public class UserRegistrationState :
    SagaStateMachineInstance
{
    public string CurrentState { get; set; } = null!;

    public DateTime RegistrationDate { get; set; }

    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public Guid CorrelationId { get; set; }
}