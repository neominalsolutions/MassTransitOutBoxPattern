using Sample.API.Contexts;
using Sample.Components;

namespace Sample.API.Services;

public interface IRegistrationService
{
    Task<UserRegisteration> SubmitRegistration(string username,string email);
}