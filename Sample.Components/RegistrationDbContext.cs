namespace Sample.Components;

using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StateMachines;


public class RegistrationDbContext :
    SagaDbContext
{


    public RegistrationDbContext(DbContextOptions<RegistrationDbContext> options)
        : base(options)
    {
    }

  protected override IEnumerable<ISagaClassMap> Configurations
  {
    get { yield return new RegistrationStateMap(); }
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    MapRegistration(modelBuilder);

    modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }

    static void MapRegistration(ModelBuilder modelBuilder)
    {
        EntityTypeBuilder<UserRegisteration> registration = modelBuilder.Entity<UserRegisteration>();

        registration.Property(x => x.Id);
        registration.HasKey(x => x.Id);

        registration.Property(x => x.RegistrationId);
        registration.Property(x => x.RegistrationDate);
        registration.Property(x => x.UserName).HasMaxLength(32);
        registration.Property(x => x.Email).HasMaxLength(32);

        registration.HasIndex(x => new
        {
            x.UserName
        }).IsUnique();
    }
}