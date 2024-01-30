namespace Sample.Components.StateMachines;

using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class RegistrationStateMap :
    SagaClassMap<UserRegistrationState>
{
    protected override void Configure(EntityTypeBuilder<UserRegistrationState> entity, ModelBuilder model)
    {
        entity.Property(x => x.CurrentState);
        entity.Property(x => x.RegistrationDate);
        entity.Property(x => x.UserName);
        entity.Property(x => x.Email);
    }
}