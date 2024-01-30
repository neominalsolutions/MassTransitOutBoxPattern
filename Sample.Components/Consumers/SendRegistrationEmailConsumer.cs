namespace Sample.Components.Consumers;

using MassTransit;
using Microsoft.Extensions.Logging;
using Sample.Components.Events;

public class SendRegistrationEmailConsumer :
    IConsumer<SendRegistrationEmail>
{
    readonly ILogger<SendRegistrationEmailConsumer> _logger;

    public SendRegistrationEmailConsumer(ILogger<SendRegistrationEmailConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<SendRegistrationEmail> context)
    {
        _logger.LogInformation($"{context.Message.Email} hesab�n�za aktivasyon maili g�nderilmi�tir.");

        return Task.CompletedTask;
    }
}