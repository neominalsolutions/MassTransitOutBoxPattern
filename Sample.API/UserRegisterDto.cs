namespace Sample.Api;

using System.ComponentModel.DataAnnotations;


public class UserRegisterDto
{
    [Required]
    public string UserName { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

}