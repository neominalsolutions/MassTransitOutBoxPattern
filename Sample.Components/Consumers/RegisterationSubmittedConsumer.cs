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
        _logger.LogInformation($"{context.Message.Username} adlý kullanýcý {context.Message.Email} mail hesabý ile sisteme kayýt olmuþtur");

        return Task.CompletedTask;
    }
}