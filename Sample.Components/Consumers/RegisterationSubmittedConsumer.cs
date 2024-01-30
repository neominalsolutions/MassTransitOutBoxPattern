namespace Sample.Components.Consumers;

using MassTransit;
using Microsoft.Extensions.Logging;
using Sample.Components.Events;

public class RegisterationSubmittedConsumer :
    IConsumer<RegistrationSubmitted>
{
    readonly ILogger<RegisterationSubmittedConsumer> _logger;

    public RegisterationSubmittedConsumer(ILogger<RegisterationSubmittedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<RegistrationSubmitted> context)
    {
        _logger.LogInformation($"{context.Message.Username} adl� kullan�c� {context.Message.Email} mail hesab� ile sisteme kay�t olmu�tur");

        return Task.CompletedTask;
    }
}