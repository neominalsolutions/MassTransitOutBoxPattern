namespace Sample.Components.Events;

public record RegistrationSubmitted
{
    public Guid RegistrationId { get; init; }
    public DateTime RegistrationDate { get; init; }
    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;
}