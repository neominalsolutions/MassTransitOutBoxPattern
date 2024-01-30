namespace Sample.API.Services;

using MassTransit;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Sample.API.Contexts;
using Sample.Components;
using Sample.Components.Events;

public class RegistrationService :
    IRegistrationService
{
    readonly RegistrationDbContext _dbContext;
    readonly IPublishEndpoint _publishEndpoint;

    public RegistrationService(RegistrationDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<UserRegisteration> SubmitRegistration(string username, string email)
    {
        var registration = new UserRegisteration
        {
            RegistrationId = NewId.NextGuid(),
            RegistrationDate = DateTime.UtcNow,
            UserName = username,
            Email = email
        };

        await _dbContext.Set<UserRegisteration>().AddAsync(registration);

        await _publishEndpoint.Publish(new RegistrationSubmitted
        {
            RegistrationId = registration.RegistrationId,
            RegistrationDate = registration.RegistrationDate,
            Username = registration.UserName,
            Email = registration.Email,
        });

        try
        {
          await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException exception) when (exception.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new Exception("Duplicate registration", exception);
        }

        return registration;
    }
}