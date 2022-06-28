using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using App.Contracts.BLL;
using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using App.Public.DTO.v1.ErrorResponses;
using App.Public.DTO.v1.Identity;
using App.Public.DTO.v1.Mappers;
using AutoMapper;
using Base.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace WebApp.ApiControllers.Identity;

[Route("api/v{version:apiVersion}/identity/[controller]/[action]")]
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class AccountController : ControllerBase
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AccountController> _logger;
    private readonly IConfiguration _configuration;
    private readonly Random _rnd = new();
    private readonly AppDbContext _context;
    private readonly IAppBLL _bll;
    private readonly PersonMapper _personMapper;


    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
        ILogger<AccountController> logger, IConfiguration configuration, AppDbContext context, IAppBLL bll, IMapper mapper)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
        _configuration = configuration;
        _context = context;
        _bll = bll;
        _personMapper = new PersonMapper(mapper);

    }

    /// <summary>
    /// Login into the rest backend - generates JWT to be included in
    /// Authorize: Bearer xyz
    /// </summary>
    /// <param name="loginData">Supply email and password</param>
    /// <returns></returns>
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(JwtResponse), 200)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(401)]
    public async Task<ActionResult<JwtResponse>> LogIn([FromBody] Login loginData)
    {
        // Verify username
        var appUser = await _userManager.FindByEmailAsync(loginData.Email);

        if (appUser == null)
        {
            _logger.LogWarning("WebApi login failed, email {} not found", loginData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "App error",
                Status = HttpStatusCode.NotFound,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["email"] =
                        new List<string>
                        {
                            "User/Password problem"
                        }
                }
            };
            // return Problem("slakfjd");
            return NotFound(errorResponse);
        }


        // Verify username & password
        var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginData.Password, false);

        if (!result.Succeeded)
        {
            _logger.LogWarning("WebApi login failed, password problem for user {}", loginData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound("User/Password problem");
        }


        // Get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);

        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get claimsPrincipal for user {}", loginData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound("User/Password problem");
        }


        // Load refresh tokens, delete expired ones
        appUser.RefreshTokens = await _context
            .Entry(appUser)
            .Collection(a => a.RefreshTokens!)
            .Query()
            .Where(t => t.AppUserId == appUser.Id)
            .ToListAsync();

        foreach (var userRefreshToken in appUser.RefreshTokens)
        {
            if (userRefreshToken.TokenExpirationDateTime < DateTime.UtcNow &&
                userRefreshToken.PreviousTokenExpirationDateTime < DateTime.UtcNow)
            {
                _context.RefreshTokens.Remove(userRefreshToken);
            }
        }


        // Create new refresh token
        var refreshToken = new RefreshToken
        {
            AppUserId = appUser.Id
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();


        // Generate jwt
        var jwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration["JWT:Key"],
            _configuration["JWT:Issuer"],
            _configuration["JWT:Issuer"],
            // DateTime.UtcNow.AddSeconds(10)
            DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JWT:ExpiresInMinutes"))
        );

        var res = new JwtResponse
        {
            Token = jwt,
            RefreshToken = refreshToken.Token,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            Email = appUser.Email
        };

        return Ok(res);
    }

    
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="registrationData">supply registration data</param>
    /// <returns></returns>
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(JwtResponse), 200)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(401)]
    public async Task<ActionResult<JwtResponse>> Register(Register registrationData)
    {
        // Verify user
        var appUser = await _userManager.FindByEmailAsync(registrationData.Email);

        if (appUser != null)
        {
            _logger.LogWarning("User with email {} is already registered", registrationData.Email);
            var errorResponse = new RestErrorResponse()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "App error",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            };
            errorResponse.Errors["email"] = new List<string>
            {
                "Email already registered"
            };
            return BadRequest(errorResponse);
        }

        var refreshToken = new RefreshToken();
        appUser = new AppUser
        {
            FirstName = registrationData.FirstName,
            LastName = registrationData.LastName,
            Email = registrationData.Email,
            UserName = registrationData.Email,
            RefreshTokens = new List<RefreshToken>
            {
                refreshToken
            }
        };


        // Create user (system will do it)
        var result = await _userManager.CreateAsync(appUser, registrationData.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        result = await _userManager.AddClaimAsync(appUser, new Claim("aspnet.firstname", appUser.FirstName));

        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        result = await _userManager.AddClaimAsync(appUser, new Claim("aspnet.lastname", appUser.LastName));

        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        var identityResultRoles = _userManager.AddToRoleAsync(appUser,
                "user")
            .Result;


        // Get full user object from Db
        appUser = await _userManager.FindByEmailAsync(appUser.Email);

        if (appUser == null)
        {
            _logger.LogWarning("User with email {} is not found after registration", registrationData.Email);
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "App error",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["email"] = new List<string>
                    {
                        $"User with email {registrationData.Email} is not found after registration"
                    }
                }
            };
            return BadRequest(errorResponse);
        }


        // Create new Person 
        // TODO: VALESTI ACCESSIMINE!
        var personDb = await _bll.Persons.FindByEmail(appUser.Email);
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(personDb?.FirstName);
        
        var person = await _context.Persons.FirstOrDefaultAsync(p => p.Email == appUser.Email);
        if (person == null)
        {
            person = new Person
            {
                AppUserId = appUser.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                IdCode = registrationData.IdCode,
                Email = appUser.Email,
                IsMain = true
            };
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            var contact = new Contact
            {
                ContactTypeId = await _bll.ContactTypes.GetEmailTypeId(),
                PersonId = person.Id,
                Value = appUser.Email
            };
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

        }
        else
        {
            person.AppUserId = appUser.Id;
            person.IsMain = true;
            _context.Persons.Update(person);
            await _context.SaveChangesAsync();
        }

        // await _context.SaveChangesAsync();


        // Get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);

        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get claimsPrincipal for user {}", appUser.Email);
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "App error",
                Status = HttpStatusCode.NotFound,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Problem"] = new List<string>
                    {
                        "User/password problem"
                    }
                }
            };
            return NotFound(errorResponse);
        }


        // Generate JWT
        var jwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration["JWT:Key"],
            _configuration["JWT:Issuer"],
            _configuration["JWT:Issuer"],
            DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JWT:ExpiresInMinutes"))
        );

        var res = new JwtResponse
        {
            Token = jwt,
            RefreshToken = refreshToken.Token,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            Email = appUser.Email
        };

        return Ok(res);
    }
    
    
    /// <summary>
    /// Creates a new refresh token for the user
    /// </summary>
    /// <param name="refreshTokenModel">Supply token and refresh token</param>
    /// <returns></returns>
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(JwtResponse), 200)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(401)]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenModel refreshTokenModel)
    {
        // Get user info from JWT
        JwtSecurityToken jwtToken;
        try
        {
            jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenModel.Jwt);

            if (jwtToken == null)
            {
                var errorResponse = new RestErrorResponse
                {
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                    Title = "App error",
                    Status = HttpStatusCode.BadRequest,
                    TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Errors =
                    {
                        ["Token"] = new List<string>
                        {
                            "No JWT token provided"
                        }
                    }
                };
                return BadRequest(errorResponse);
            }
        }
        catch (Exception e)
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "App error",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Token"] = new List<string>
                    {
                        $"Cannot parse the token, {e.Message}"
                    }
                }
            };
            return BadRequest(errorResponse);
        }


        // TODO: validate token signature

        var userEmail = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        if (userEmail == null)
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231/#section-6.5.1",
                Title = "App error",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["No email"] = new List<string>
                    {
                        "No email address provided"
                    }
                }
            };
            return BadRequest(errorResponse);
        }


        // Get user and tokens
        var appUser = await _userManager.FindByEmailAsync(userEmail);

        if (appUser == null)
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231/#section-6.5.4",
                Title = "App error",
                Status = HttpStatusCode.NotFound,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["No email"] = new List<string>
                    {
                        $"User with email {userEmail} not found"
                    }
                }
            };
            return NotFound($"User with email {userEmail} not found");
        }


        // Load and compare refresh tokens
        await _context.Entry(appUser)
            .Collection(u => u.RefreshTokens!)
            .Query()
            .Where(x =>
                (x.Token == refreshTokenModel.RefreshToken && x.TokenExpirationDateTime > DateTime.UtcNow) ||
                (x.PreviousToken == refreshTokenModel.RefreshToken &&
                 x.PreviousTokenExpirationDateTime > DateTime.UtcNow))
            .ToListAsync();

        if (appUser.RefreshTokens == null)
        {
            return Problem("RefreshTokens collection is null");
        }

        if (appUser.RefreshTokens.Count == 0)
        {
            return Problem("RefreshTokens collection is empty, no valid refresh tokens found");
        }

        if (appUser.RefreshTokens.Count != 1)
        {
            return Problem("More than one valid refresh token found");
        }


        // Get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);

        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get claimsPrincipal for user {}", userEmail);


            return NotFound("User/Password problem");
        }

        // Generate new JWT
        var jwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration["JWT:Key"],
            _configuration["JWT:Issuer"],
            _configuration["JWT:Issuer"],
            DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JWT:ExpiresInMinutes"))
        );


        // Create new refresh token, obsolete old ones
        var refreshToken = appUser.RefreshTokens.First();

        if (refreshToken.Token == refreshTokenModel.RefreshToken)
        {
            refreshToken.PreviousToken = refreshToken.Token;
            refreshToken.PreviousTokenExpirationDateTime = DateTime.UtcNow.AddMinutes(1);

            refreshToken.Token = Guid.NewGuid().ToString();
            refreshToken.TokenExpirationDateTime = DateTime.Now.AddDays(7);

            await _context.SaveChangesAsync();
        }

        var res = new JwtResponse
        {
            Token = jwt,
            RefreshToken = refreshToken.Token,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            Email = appUser.Email
        };


        return Ok(res);
    }
}