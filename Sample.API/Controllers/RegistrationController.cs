namespace Sample.Api.Controllers;


using Microsoft.AspNetCore.Mvc;
using Sample.API.Services;
using Sample.Components;

[ApiController]
[Route("[controller]")]
public class RegistrationController :
    ControllerBase
{
    readonly IRegistrationService _registrationService;

    public RegistrationController(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    // https://masstransit.io/documentation/concepts/messages ziyaret edelim.

  [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var registration = await _registrationService.SubmitRegistration(model.UserName, model.Email);

            return Ok(new
            {
                registration.RegistrationId,
                registration.RegistrationDate,
                registration.UserName,
                registration.Email,
            });
        }
        catch (DuplicateRegistrationException)
        {
            return Conflict(new
            {
                model.Email,
                model.UserName,
            });
        }
    }
}