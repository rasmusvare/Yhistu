using System.ComponentModel.DataAnnotations;

namespace App.Public.DTO.v1.Identity;

public class Register
{
    [StringLength(maximumLength: 128, MinimumLength = 5, ErrorMessage = "Wrong length on email")]
    public string Email { get; set; } = default!;

    [StringLength(maximumLength: 128, MinimumLength = 1, ErrorMessage = "Wrong length on password")]
    public string Password { get; set; } = default!;

    [StringLength(maximumLength: 128, MinimumLength = 1, ErrorMessage = "Wrong length on first name")]
    public string FirstName { get; set; } = default!;

    [StringLength(maximumLength: 128, MinimumLength = 1, ErrorMessage = "Wrong length on last name")]
    public string LastName { get; set; } = default!;
    
    [StringLength(maximumLength: 32, MinimumLength = 11, ErrorMessage = "Wrong length on ID code")]
    public string IdCode { get; set; } = default!;
}