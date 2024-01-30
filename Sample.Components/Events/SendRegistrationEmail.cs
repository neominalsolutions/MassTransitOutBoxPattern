namespace Sample.Components.Events;

public record SendRegistrationEmail
{
    public Guid RegistrationId { get; init; }
    public DateTime RegistrationDate { get; init; }
    public string Email { get; init; } = null!;
}