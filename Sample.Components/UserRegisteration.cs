namespace Sample.Components;

public class UserRegisteration
{
    public int Id { get; set; }
    public Guid RegistrationId { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
}