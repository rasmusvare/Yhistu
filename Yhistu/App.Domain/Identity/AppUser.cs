using System.ComponentModel.DataAnnotations;
using Base.Domain.Identity;

namespace App.Domain.Identity;

public class AppUser : BaseUser
{
    [MinLength(1)] [MaxLength(128)] public string FirstName { get; set; } = default!;

    [MinLength(1)] [MaxLength(128)] public string LastName { get; set; } = default!;

    // Fields connected to user

    public ICollection<Person>? Persons { get; set; }

    public ICollection<RefreshToken>? RefreshTokens { get; set; }
    

    public string FirstLastName => FirstName + " " + LastName;
    public string LastFirstName => LastName + " " + FirstName;
}